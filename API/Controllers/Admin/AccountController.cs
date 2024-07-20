using API.Dtos.Admin;
using API.Helpers;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AccountController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserToReturnDto>>> GetUsers([FromQuery] AdminSpecParams adminParams)
        {
            var totalItems = _userManager.Users.Count();

            var users = await _userManager.Users.Skip((adminParams.PageIndex - 1) * adminParams.PageSize).Take(adminParams.PageSize).ToListAsync();

            List<UserToReturnDto> data = new List<UserToReturnDto>();

            foreach (var user in users)
            {
                var userToReturnDto = _mapper.Map<UserToReturnDto>(user);

                var roles = await _userManager.GetRolesAsync(user);
                var gradeCode = roles.OrderByDescending(x => (int)((RoleType)Enum.Parse(typeof(RoleType), x, true))).FirstOrDefault();
                userToReturnDto.Grade = gradeCode;
                userToReturnDto.IsAdmin = roles.Contains(RoleType.Admin.ToString());
                data.Add(userToReturnDto);
            }

            return Ok(new Pagination<UserToReturnDto>(adminParams.PageIndex, adminParams.PageSize, totalItems, data.AsReadOnly()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserToReturnDto>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userToReturnDto = _mapper.Map<UserToReturnDto>(user);

            var roles = await _userManager.GetRolesAsync(user);
            var gradeCode = roles.OrderByDescending(x => (int)((RoleType)Enum.Parse(typeof(RoleType), x))).FirstOrDefault();
            userToReturnDto.Grade = gradeCode;
            userToReturnDto.IsAdmin = roles.Contains(RoleType.Admin.ToString());
            return userToReturnDto;
        }

        [HttpPost]
        public async Task<ActionResult<UserToReturnDto>> CreateUser(UserDto userDto)
        {
            var user = new AppUser
            {
                UserName = userDto.UserName,
                DisplayName = userDto.DisplayName,
                PhoneNumber = userDto.PhoneNumber,
                Email = userDto.Email,
                Address = new Address
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    County = string.Empty,
                    City = string.Empty,
                    Street = string.Empty,
                    ZipCode = string.Empty,
                    PhoneNumber = string.Empty
                }
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Problem create the user");
            }

            var roles = new List<string>();

            if (userDto.IsAdmin)
            {
                roles.Add(nameof(RoleType.Admin));
            }

            if (userDto.Grade != string.Empty)
            {
                roles.Add(userDto.Grade);
            }

            result = await _userManager.AddToRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                return BadRequest("Problem add the user role");
            }

            return Ok(_mapper.Map<UserToReturnDto>(user));
        }

        [HttpPut]
        public async Task<ActionResult<UserToReturnDto>> UpdateUser(UserDto userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.UserName);

            user.DisplayName = userDto.DisplayName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Email = userDto.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest("Problem updating the user");
            }

            if (userDto.Password != string.Empty)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                result = await _userManager.ResetPasswordAsync(user, token, userDto.Password);

                if (!result.Succeeded)
                {
                    return BadRequest("Problem updating the user password");
                }
            }

            var oldRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRoles);

            var roles = new List<string>();

            if (userDto.IsAdmin)
            {
                roles.Add(nameof(RoleType.Admin));
            }

            if (userDto.Grade != string.Empty)
            {
                roles.Add(userDto.Grade);
            }

            result = await _userManager.AddToRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                return BadRequest("Problem updating the user role");
            }

            return Ok(_mapper.Map<UserToReturnDto>(user));
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return Ok(true);
        }
    }
}