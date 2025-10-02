using System.ComponentModel;
using Core.Contracts;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace Infrastructure.AIPlugins
{
    public class ProductPlugin
    {
        private readonly IServiceProvider _serviceProvider;
        public ProductPlugin(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [KernelFunction("get_products")]
        [Description("取得產品清單")]
        public async Task<List<ProductSummaryContract>> GetProducts()
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            ProductSpecParams productParams = new ProductSpecParams { PageSize = 50 };
            var spec = new ProductWithFiltersAndAllowShowingSpecificication(productParams);
            var products = await uow.Repository<Product>().ListAsync(spec);
            
            return mapper.Map<List<ProductSummaryContract>>(products);
        }

        [KernelFunction("get_product")]
        [Description("取得產品詳細資訊")]
        public async Task<ProductDetailContract> GetProduct(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var spec = new ProductByIdAndAllowShowingSpecification(id);
            var product = await uow.Repository<Product>().GetEntityWithSpec(spec);
            
            return mapper.Map<ProductDetailContract>(product);
        }

        [KernelFunction("get_bestseller")]
        [Description("取得熱銷商品")]
        public async Task<List<ProductSummaryContract>> GetBestseller()
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var spec = new ProductIsBestsellerAndAllowShowingSpecification();
            var products = await uow.Repository<Product>().ListAsync(spec);

            return mapper.Map<List<ProductSummaryContract>>(products);
        }

        [KernelFunction("get_product_types")]
        [Description("取得產品類型清單")]
        public async Task<List<ProductTypeContract>> GetProductTypes()
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var productTypes = await uow.Repository<ProductType>().ListAllAsync();

            return mapper.Map<List<ProductTypeContract>>(productTypes);
        }
    }
}