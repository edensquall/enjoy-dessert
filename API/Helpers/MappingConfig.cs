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
            .Map(d => d.ThumbnailUrl, s => s.ProductImages.Any(x => x.Order == 1)
                ? $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}product/{s.Id}/{s.ProductImages.Where(x => x.Order == 1).First().Name}"
                : string.Empty)
            .Map(d => d.ImageUrls, s => s.ProductImages.Select(x => $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}product/{s.Id}/{x.Name}"));

            config.NewConfig<AppUser, UserDto>()
            .Map(d => d.UserName, s => s.UserName)
            .Map(d => d.DisplayName, s => s.DisplayName)
            .Map(d => d.Token, s => MapContext.Current.GetService<ITokenService>().CreateToken(s));

            config.NewConfig<News, NewsDto>()
            .Map(d => d.ThumbnailUrl, s => $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}news/{s.Id}/{s.Thumbnail}");

            config.NewConfig<Slide, SlideDto>()
            .Map(d => d.ImageUrl, s => $"{MapContext.Current.GetService<IConfiguration>().GetValue<string>("ApiUrl")}slide/{s.Image}");

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