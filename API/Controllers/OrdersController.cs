using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var userName = User.RetrieveUserNameFromPrincipal();

            var address = _mapper.Map<Address>(orderDto.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(userName, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<OrderToReturnDto>>> GetOrdersForUser([FromQuery] OrderSpecParams orderParams)
        {
            var userName = User.RetrieveUserNameFromPrincipal();

            var orders = await _orderService.GetOrderForUserAsync(userName, orderParams);

            var totalItems = await _orderService.GetOrderCountForUserAsync(userName);

            var data = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);

            return Ok(new Pagination<OrderToReturnDto>(orderParams.PageIndex, orderParams.PageSize, totalItems, data));
        }

        [HttpGet("lastOrder")]
        public async Task<ActionResult<OrderToReturnDto>> GetLastOrderForUser()
        {
            var userName = User.RetrieveUserNameFromPrincipal();

            var order = await _orderService.GetOrderForUserAsync(userName);

            return _mapper.Map<OrderToReturnDto>(order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var userName = User.RetrieveUserNameFromPrincipal();

            var order = await _orderService.GetOrderByIdAsync(id, userName);

            if (order == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<OrderToReturnDto>(order);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}