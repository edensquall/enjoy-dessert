using System.ComponentModel;
using Infrastructure.Contracts;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Agent.Contracts;

namespace Infrastructure.Agent.Tools
{
    public class ProductTool
    {
        private readonly IServiceProvider _serviceProvider;
        public ProductTool(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [Description("取得產品清單")]
        public async Task<ResultContract<List<ProductSummaryContract>>> GetProducts()
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            ProductSpecParams productParams = new ProductSpecParams { PageSize = 50 };
            var spec = new ProductWithFiltersAndAllowShowingSpecificication(productParams);
            var products = await uow.Repository<Product>().ListAsync(spec);

            return ResultContract<List<ProductSummaryContract>>.Ok(mapper.Map<List<ProductSummaryContract>>(products));
        }

        [Description("取得產品詳細資訊")]
        public async Task<ResultContract<ProductDetailContract>> GetProduct([Description("產品ID")] int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var spec = new ProductByIdAndAllowShowingSpecification(id);
            var product = await uow.Repository<Product>().GetEntityWithSpec(spec);

            return ResultContract<ProductDetailContract>.Ok(mapper.Map<ProductDetailContract>(product));
        }

        [Description("取得熱銷商品")]
        public async Task<ResultContract<List<ProductSummaryContract>>> GetBestseller()
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var spec = new ProductIsBestsellerAndAllowShowingSpecification();
            var products = await uow.Repository<Product>().ListAsync(spec);

            return ResultContract<List<ProductSummaryContract>>.Ok(mapper.Map<List<ProductSummaryContract>>(products));
        }

        [Description("取得產品類型清單")]
        public async Task<ResultContract<List<ProductTypeContract>>> GetProductTypes()
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var productTypes = await uow.Repository<ProductType>().ListAllAsync();

            return ResultContract<List<ProductTypeContract>>.Ok(mapper.Map<List<ProductTypeContract>>(productTypes));
        }
    }
}