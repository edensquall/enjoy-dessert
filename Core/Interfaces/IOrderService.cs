using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerUserName, int deliveryMethod, string basketId, Address shippingAddress);
        Task<Order> GetOrderForUserAsync(string buyerUserName);
        Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerUserName, OrderSpecParams orderParams);
        Task<int> GetOrderCountForUserAsync(string buyerUserName);
        Task<Order> GetOrderByIdAsync(int id, string buyerUserName);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}