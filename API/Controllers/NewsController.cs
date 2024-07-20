using API.Dtos;
using API.Errors;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public NewsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<NewsDto>>> GetNewsAll(
            [FromQuery] NewsSpecParams newsParams)
        {
            var spec = new NewsAllowShowingSpecification(newsParams);

            var countSpec = new NewsAllowShowingSpecification();

            var totalItems = await _unitOfWork.Repository<News>().CountAsync(countSpec);

            var news = await _unitOfWork.Repository<News>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<NewsDto>>(news);

            return Ok(new Pagination<NewsDto>(newsParams.PageIndex, newsParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsDto>> GetNews(int id)
        {
            var spec = new NewsAllowShowingSpecification(id);

            var news = await _unitOfWork.Repository<News>().GetEntityWithSpec(spec);

            if (news == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<NewsDto>(news));
        }
    }
}