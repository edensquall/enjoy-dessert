using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> orderItems, string? buyerUserName, Address? shipToAddress,
        DeliveryMethod? deliveryMethod, decimal subtotal, string paymentIntentId)
        {
            BuyerUserName = buyerUserName;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string? BuyerUserName { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Required]
        public Address? ShipToAddress { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? PaymentIntentId { get; set; }
        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod!.Price;
        }
    }
}