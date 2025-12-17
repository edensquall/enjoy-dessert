using API.Dtos;
using API.Extensions;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Mapster;

namespace API.Helpers
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Product, ProductToReturnDto>()
            .Map(d => d.Type, s => s.ProductType.Name)
            .Map(d => d.ThumbnailUrl, s => s.ProductImages.Count == 0 ? null : s.ProductImages.Any(x => x.Order == 1) ?
            $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}product/{s.Id}/{s.ProductImages.Where(x => x.Order == 1).First().Name}" : null )
            .Map(d => d.ImageUrls, s => s.ProductImages.Count == 0 ? null : s.ProductImages.Select(x => string.IsNullOrEmpty(x.Name) ? null : $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}product/{s.Id}/{x.Name}"));

            config.NewConfig<Product, API.Dtos.Admin.ProductToReturnDto>()
            .Map(d => d.ImageUrls, s => s.ProductImages.OrderBy(x => x.Order).Select(x => $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}product/{s.Id}/{x.Name}"))
            .Map(d => d.ImageUrlsIndex, s => s.ProductImages.OrderBy(x => x.Order).Select(x => x.Order));

            config.NewConfig<News, NewsDto>()
            .Map(d => d.ThumbnailUrl, s => string.IsNullOrEmpty(s.Thumbnail) ? null : $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}news/{s.Id}/{s.Thumbnail}");

            config.NewConfig<News, API.Dtos.Admin.NewsToReturnDto>()
            .Map(d => d.ThumbnailUrl, s => string.IsNullOrEmpty(s.Thumbnail) ? null : $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}news/{s.Id}/{s.Thumbnail}");

            config.NewConfig<API.Dtos.Admin.NewsDto, News>()
            .IgnoreIf((s, d) => string.IsNullOrEmpty(s.Thumbnail), d => d.Thumbnail);

            config.NewConfig<Slide, SlideDto>()
            .Map(d => d.ImageUrl, s => $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}slide/{s.Image}");

            config.NewConfig<Order, API.Dtos.Admin.OrderToReturnDto>()
            .Map(d => d.DeliveryMethod, s => s.DeliveryMethod.ShortName)
            .Map(d => d.ShippingPrice, s => s.DeliveryMethod.Price)
            .Map(d => d.Status, s => s.Status);

            config.NewConfig<OrderItem, API.Dtos.Admin.OrderItemDto>()
            .Map(d => d.ProductId, s => s.ItemOrdered.ProductItemId)
            .Map(d => d.ProductName, s => s.ItemOrdered.ProductName)
            .Map(d => d.ImageUrl, s => s.ItemOrdered.ImageUrl);

            config.NewConfig<Order, OrderToReturnDto>()
            .Map(d => d.DeliveryMethod, s => s.DeliveryMethod.ShortName)
            .Map(d => d.ShippingPrice, s => s.DeliveryMethod.Price)
            .Map(d => d.Status, s => s.Status.GetDescription());

            config.NewConfig<OrderItem, OrderItemDto>()
            .Map(d => d.ProductId, s => s.ItemOrdered.ProductItemId)
            .Map(d => d.ProductName, s => s.ItemOrdered.ProductName)
            .Map(d => d.ImageUrl, s => s.ItemOrdered.ImageUrl);
        }
    }
}