using API.Dtos;
using API.Errors;
using API.Extensions;
using Core.Entities.Identity;
using Core.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByUserNameFromClaimsPrinciple(User);
            return _mapper.Map<UserDto>(user);
        }

        [Authorize]
        [HttpGet("userinfo")]
        public async Task<ActionResult<UserInfoDto>> GetUserInfo()
        {
            var user = await _userManager.FindByUserNameFromClaimsPrinciple(User);
            var userInfo = _mapper.Map<UserInfoDto>(user);
            var gradeCode = _userManager.GetRolesAsync(user).Result.OrderByDescending(x => (int)((RoleType)Enum.Parse(typeof(RoleType), x))).FirstOrDefault();
            userInfo.GradeTitle = ((RoleType)Enum.Parse(typeof(RoleType), gradeCode)).GetDescription();
            return userInfo;
        }

        [Authorize]
        [HttpPut("userinfo")]
        public async Task<ActionResult<UserInfoDto>> UpdateUserInfo(UserInfoDto userInfo)
        {
            var user = await _userManager.FindByUserNameFromClaimsPrinciple(User);

            user.DisplayName = userInfo.DisplayName;
            user.PhoneNumber = userInfo.PhoneNumber;
            user.Email = userInfo.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<UserInfoDto>(user));

            return BadRequest("Problem updating the user info");
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<ActionResult<bool>> UpdatePassword(PasswordDto password)
        {
            var user = await _userManager.FindByUserNameFromClaimsPrinciple(User);

            if (CheckPasswordValidAsync(password.Password).Result.Value == false)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                {"當前密碼錯誤"}
                });
            }

            var result = await _userManager.ChangePasswordAsync(user, password.Password, password.NewPassword);

            if (result.Succeeded) return Ok(true);

            return BadRequest("Problem updating the user password");
        }

        [HttpGet("passwrodValid")]
        public async Task<ActionResult<bool>> CheckPasswordValidAsync([FromQuery] string password)
        {
            var user = await _userManager.FindByUserNameFromClaimsPrinciple(User);

            return await _userManager.CheckPasswordAsync(user, password);
        }


        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("usernameexists")]
        public async Task<ActionResult<bool>> CheckUserNameExistsAsync([FromQuery] string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindByUserByClaimsPrincipleWithAddressAsync(User);

            return _mapper.Map<AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindByUserByClaimsPrincipleWithAddressAsync(User);

            user.Address = _mapper.Map<Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));

            return BadRequest("Problem updating the user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "帳號密碼錯誤，請重新登入" }
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "帳號密碼錯誤，請重新登入" }
                });
            }

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = await _tokenService.CreateTokenAsync(user);
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
            Response.Cookies.Append("refreshToken", refreshToken.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = refreshToken.ExpiresAt
                });

            return userDto;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                {"Email 已經被使用過了"}
                });
            }

            if (CheckUserNameExistsAsync(registerDto.UserName).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                {"帳號 已經被使用過了"}
                });
            }

            var user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, nameof(RoleType.RegularMember)).Wait();
            }

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = await _tokenService.CreateTokenAsync(user);

            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
            Response.Cookies.Append("refreshToken", refreshToken.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = refreshToken.ExpiresAt
                });

            return userDto;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return Ok();

            await _tokenService.RevokeRefreshTokenAsync(refreshToken, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");

            Response.Cookies.Append("refreshToken", "",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(-1)
                });
            return Ok();
        }

        [HttpPost("refreshtoken")]
        public async Task<ActionResult<RefreshTokenDto>> RefreshToken()
        {
            var refreshTokenString = Request.Cookies["refreshToken"] ?? string.Empty;

            var refreshToken = await _tokenService.RefreshTokenAsync(refreshTokenString, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");

            if (refreshToken == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                    {"refresh token 驗證失敗"}
                });
            }

            Response.Cookies.Append("refreshToken", refreshToken.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = refreshToken.ExpiresAt
                });

            return Ok(new RefreshTokenDto
            {
                Token = await _tokenService.CreateTokenAsync(refreshToken.AppUser)
            });
        }
    }
}