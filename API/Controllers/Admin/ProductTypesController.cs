using API.Dtos.Admin;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ProductTypesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ProductTypesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ProductTypeDto>> CreateProductType(ProductTypeDto productTypeDto)
        {
            var productType = new ProductType()
            {
                Name = productTypeDto.Name
            };

            _unitOfWork.Repository<ProductType>().Add(productType);
            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating product type"));

            return Ok(_mapper.Map<ProductTypeDto>(productType));
        }

        [HttpPut]
        public async Task<ActionResult<ProductTypeDto>> UpdateProductType(ProductTypeDto productTypeDto)
        {
            var productType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(productTypeDto.Id);
            if (productType != null)
            {
                _mapper.Map<ProductTypeDto, ProductType>(productTypeDto, productType);
                var result = await _unitOfWork.Complete();

                if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating product type"));
            }

            return Ok(_mapper.Map<ProductTypeDto>(productType));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductTypeDto>>> GetProductTypes()
        {
            var productTypes = await _unitOfWork.Repository<ProductType>().ListAllAsync();

            return Ok(_mapper.Map<IReadOnlyList<ProductType>>(productTypes.OrderBy(x => x.Id)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTypeDto>> GetProductType(int id)
        {
            var productType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);

            if (productType == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<ProductTypeDto>(productType));
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteProductType(int id)
        {
            var productType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);

            if (productType != null)
            {
                _unitOfWork.Repository<ProductType>().Delete(productType);
                await _unitOfWork.Complete();
            }

            return Ok(true);
        }
    }
}