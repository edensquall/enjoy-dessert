using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlidesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public SlidesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SlideDto>>> GetSlides()
        {
            var slides = await _unitOfWork.Repository<Slide>().ListAllAsync();

            return Ok(_mapper.Map<IReadOnlyList<SlideDto>>(slides));
        }
    }
}