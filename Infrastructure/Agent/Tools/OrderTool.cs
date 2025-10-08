using System.ComponentModel;
using Infrastructure.Contracts;
using Core.Interfaces;
using Core.Specifications;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Agent.Contracts;

namespace Infrastructure.Agent.Tools
{
    public class OrderTool
    {
        private readonly IServiceProvider _serviceProvider;
        public OrderTool(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [Description("取得使用者的訂單清單")]
        public async Task<ResultContract<List<OrderSummaryContract>>> GetCurrentUserOrders([Description("使用者名稱")] string userName)
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

            if (userName == null) return ResultContract<List<OrderSummaryContract>>.Fail("使用者未登入");

            OrderSpecParams orderParams = new OrderSpecParams { PageSize = 50 };

            var orders = await orderService.GetOrderForUserAsync(userName, orderParams);

            return ResultContract<List<OrderSummaryContract>>.Ok(mapper.Map<List<OrderSummaryContract>>(orders));

        }

        [Description("取得使用者的詳細訂單")]
        public async Task<ResultContract<OrderDetailContract>> GetCurrentUserOrder([Description("使用者名稱")] string userName, [Description("訂單ID")] int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

            if (userName == null) return ResultContract<OrderDetailContract>.Fail("使用者未登入");

            var order = await orderService.GetOrderByIdAsync(id, userName);

            return ResultContract<OrderDetailContract>.Ok(mapper.Map<OrderDetailContract>(order));
        }
    }
}