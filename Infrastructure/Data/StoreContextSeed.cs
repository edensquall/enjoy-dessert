using System.Reflection;
using System.Text.Json;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!context.ProductTypes.Any())
                {
                    var productTypesData = File.ReadAllText(path + @"/Data/SeedData/productTypes.json");

                    var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypesData);
                    context.AddRange(productTypes);
                }

                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText(path + @"/Data/SeedData/products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    context.AddRange(products);
                }

                if (!context.ProductImages.Any())
                {
                    var productImagesData = File.ReadAllText(path + @"/Data/SeedData/productImages.json");

                    var productImages = JsonSerializer.Deserialize<List<ProductImage>>(productImagesData);
                    context.AddRange(productImages);
                }

                if (!context.News.Any())
                {
                    var newsData = File.ReadAllText(path + @"/Data/SeedData/news.json");

                    var news = JsonSerializer.Deserialize<List<News>>(newsData);
                    context.AddRange(news);
                }

                if (!context.Slides.Any())
                {
                    var slidesData = File.ReadAllText(path + @"/Data/SeedData/slides.json");

                    var slides = JsonSerializer.Deserialize<List<Slide>>(slidesData);
                    context.AddRange(slides);
                }
                if (!context.DeliveryMethods.Any())
                {
                    var dmData = File.ReadAllText(path + @"/Data/SeedData/delivery.json");

                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    context.AddRange(methods);
                }

                if (context.ChangeTracker.HasChanges())
                {
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}