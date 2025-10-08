using Core.Entities.OrderAggregate;
using Infrastructure.Contracts;
using Mapster;
using Infrastructure.Extensions;

namespace API.Helpers
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Order, OrderSummaryContract>()
            .Map(d => d.Status, s => s.Status.GetDescription());

            config.NewConfig<Order, OrderDetailContract>()
            .Map(d => d.DeliveryMethod, s => s.DeliveryMethod.ShortName)
            .Map(d => d.Status, s => s.Status.GetDescription());

            config.NewConfig<OrderItem, OrderItemContract>()
            .Map(d => d.ProductName, s => s.ItemOrdered.ProductName);
        }
    }
}