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
    public class ProductsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndImagesSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecificication(productParams);


            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [Cached(600)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndImagesSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _unitOfWork.Repository<ProductType>().ListAllAsync());
        }

        [Cached(600)]
        [HttpGet("bestseller")]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetBestseller()
        {
            var spec = new ProductsIsBestsellerWithTypesAndImagesSpecification();

            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<ProductToReturnDto>>(products));
        }
    }
}