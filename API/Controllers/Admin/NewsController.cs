using API.Dtos.Admin;
using API.Errors;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    // [Authorize(Roles = "Admin")]
    public class NewsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public NewsController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("uploadNewsDetailImage/{id}")]
        public async Task<ActionResult<ImageToReturnDto>> UploadNewsDetailImage(string id, IFormFile file)
        {
            ImageToReturnDto imageToReturnDto = new ImageToReturnDto();
            if (file.Length > 0)
            {
                var path = string.Empty;
                if (id.Contains('-'))
                {
                    path = $@"{_config.GetValue<string>("UploadFile")}temp/news/{id}/detail/";
                    imageToReturnDto.Location = $@"{_config.GetValue<string>("ApiUrl")}temp/news/{id}/detail/{file.FileName}";
                }
                else
                {
                    path = $@"{_config.GetValue<string>("UploadFile")}news/{id}/detail/";
                    imageToReturnDto.Location = $@"{_config.GetValue<string>("ApiUrl")}news/{id}/detail/{file.FileName}";
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (var fileStream = new FileStream(path + file.FileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return Ok(imageToReturnDto);
        }

        [HttpPost]
        public async Task<ActionResult<NewsToReturnDto>> CreateNews([FromForm] NewsDto newsDto)
        {
            newsDto.Thumbnail = newsDto.ThumbnailFile?.FileName;
            var news = _mapper.Map<News>(newsDto);
            _unitOfWork.Repository<News>().Add(news);
            var result = await _unitOfWork.Complete();

            if (news.Content.Contains($@"temp/news/{newsDto.TempId}/"))
            {
                news.Content = news?.Content?.Replace(
                    $@"temp/news/{newsDto.TempId}/",
                    $@"news/{news.Id}/");
                await _unitOfWork.Complete();
            }

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating news"));

            var tempPath = $@"{_config.GetValue<string>("UploadFile")}temp/news/{newsDto.TempId}/";
            var path = $@"{_config.GetValue<string>("UploadFile")}news/{news.Id}/";

            if (Directory.Exists(tempPath))
            {
                Directory.Move(tempPath, path);
            }

            if (newsDto.ThumbnailFile != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (var fileStream = new FileStream(path + newsDto.ThumbnailFile.FileName, FileMode.Create))
                {
                    await newsDto.ThumbnailFile.CopyToAsync(fileStream);
                }
            }

            return Ok(_mapper.Map<NewsToReturnDto>(news));
        }

        [HttpPut]
        public async Task<ActionResult<NewsToReturnDto>> UpdateNews([FromForm] NewsDto newsDto)
        {
            var news = await _unitOfWork.Repository<News>().GetByIdAsync(newsDto.Id);
            if (news != null)
            {
                newsDto.Thumbnail = newsDto.ThumbnailFile?.FileName;
                _mapper.Map<NewsDto, News>(newsDto, news);
                var result = await _unitOfWork.Complete();
                if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating news"));

                if (newsDto.ThumbnailFile != null)
                {
                    var path = $@"{_config.GetValue<string>("UploadFile")}news/{news.Id}/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (var fileStream = new FileStream(path + newsDto.ThumbnailFile.FileName, FileMode.Create))
                    {
                        await newsDto.ThumbnailFile.CopyToAsync(fileStream);
                    }
                }
            }

            return Ok(_mapper.Map<NewsToReturnDto>(news));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<NewsToReturnDto>>> GetNewsAll(
            [FromQuery] NewsSpecParams NewsParams)
        {
            var spec = new NewsSpecification(NewsParams);

            var totalItems = (await _unitOfWork.Repository<News>().ListAllAsync()).Count;

            var news = await _unitOfWork.Repository<News>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<NewsToReturnDto>>(news);

            return Ok(new Pagination<NewsToReturnDto>(NewsParams.PageIndex, NewsParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsToReturnDto>> GetNews(int id)
        {
            var news = await _unitOfWork.Repository<News>().GetByIdAsync(id);

            if (news == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<NewsToReturnDto>(news));
        }

        [HttpDelete]
        public async Task DeleteNews(int id)
        {
            var news = await _unitOfWork.Repository<News>().GetByIdAsync(id);
            if (news != null)
            {
                _unitOfWork.Repository<News>().Delete(news);
                var result = await _unitOfWork.Complete();

                var path = $@"{_config.GetValue<string>("UploadFile")}news/{id}/";
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
        }
    }
}