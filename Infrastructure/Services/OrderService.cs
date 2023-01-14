using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _config;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork, IPaymentService paymentService, IConfiguration config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerUserName, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // get basket from the repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // get items from the product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productsWithTypesAndImagesSpec = new ProductsWithTypesAndImagesSpecification(item.Id);
                var productItem = await _unitOfWork.Repository<Product>().GetEntityWithSpec(productsWithTypesAndImagesSpec);
                var productItemImageUrl = $"{_config["ApiUrl"]}product/{productItem.Id}/{productItem.ProductImages.Where(x => x.Order == 1).FirstOrDefault()?.Name}" ?? string.Empty;
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItemImageUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            // get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            // check to see if order exists
            var orderByPaymentIntentIdSpec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(orderByPaymentIntentIdSpec);

            if (existingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
            }

            // create order
            var order = new Order(items, buyerUserName, shippingAddress, deliveryMethod, subtotal, basket.PaymentIntentId);
            _unitOfWork.Repository<Order>().Add(order);

            // TODO: save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            // return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerUserName)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerUserName);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<Order> GetOrderForUserAsync(string buyerUserName)
        {
            var spec = new OrderByBuyerUserNameSpecification(buyerUserName);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }
        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerUserName, OrderSpecParams orderParams)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerUserName, orderParams);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }

        public async Task<int> GetOrderCountForUserAsync(string buyerUserName)
        {
            var countSpec = new OrderByBuyerUserNameSpecification(buyerUserName);

            return await _unitOfWork.Repository<Order>().CountAsync(countSpec);
        }
    }
}