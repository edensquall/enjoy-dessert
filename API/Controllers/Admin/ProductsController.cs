using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ProductToReturnDto>> CreateProduct([FromForm] ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            if (productDto.ImageFilesIndex != null)
            {
                for (var i = 0; i < productDto.ImageFilesIndex.Length; i++)
                {
                    product.ProductImages.Add(new ProductImage(productDto.ImageFiles[i].FileName, productDto.ImageFilesIndex[i] + 1));
                }
            }
            var productType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(productDto.ProductTypeId);

            _unitOfWork.Repository<Product>().Add(product);
            var result = await _unitOfWork.Complete();

            var path = $@"{_config.GetValue<string>("UploadFile")}product/{product.Id}/";

            if (productDto.ImageFiles != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (IFormFile file in productDto.ImageFiles)
                {
                    using (var fileStream = new FileStream(path + file.FileName, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [HttpPut]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProduct([FromForm] ProductDto productDto)
        {
            var spec = new ProductByIdSpecification(productDto.Id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
            if (product != null)
            {
                _mapper.Map<ProductDto, Product>(productDto, product);
                if (productDto.ImageFilesIndex != null)
                {
                    for (var i = 0; i < productDto.ImageFilesIndex.Length; i++)
                    {
                        var index = product.ProductImages.FindIndex(x => x.Order == productDto.ImageFilesIndex[i] + 1);
                        if(index != -1)
                        {
                            product.ProductImages[index] = new ProductImage(productDto.ImageFiles[i].FileName, productDto.ImageFilesIndex[i] + 1);
                        }
                        else
                        {
                            product.ProductImages.Add(new ProductImage(productDto.ImageFiles[i].FileName, productDto.ImageFilesIndex[i] + 1));
                        }
                    }
                }


                var result = await _unitOfWork.Complete();

                var path = $@"{_config.GetValue<string>("UploadFile")}product/{product.Id}/";

                if (productDto.ImageFiles != null)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    foreach (IFormFile file in productDto.ImageFiles)
                    {
                        using (var fileStream = new FileStream(path + file.FileName, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }

            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductWithFiltersSpecificication(productParams);

            var countSpec = new ProductWithFiltersForCountSpecificication(productParams);

            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductByIdSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            product.ProductImages = product.ProductImages.OrderBy(x => x.Order).ToList();

            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [HttpDelete]
        public async Task DeleteProduct(int id)
        {
            var spec = new ProductByIdSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
            if (product != null)
            {
                _unitOfWork.Repository<Product>().Delete(product);
                var result = await _unitOfWork.Complete();

                var path = $@"{_config.GetValue<string>("UploadFile")}product/{id}/";
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _unitOfWork.Repository<ProductType>().ListAllAsync());
        }
    }
}