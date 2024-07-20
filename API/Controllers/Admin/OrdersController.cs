using API.Dtos.Admin;
using API.Errors;
using API.Helpers;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPut]
        public async Task<ActionResult<API.Dtos.Admin.OrderToReturnDto>> UpdateOrder(OrderDto orderDto)
        {
            var spec = new OrderByIdSpecification(orderDto.Id);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if (order != null)
            {
                order.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), orderDto.Status);
                _unitOfWork.Repository<Order>().Update(order);

                var result = await _unitOfWork.Complete();

                if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating order"));
            }

            return Ok(_mapper.Map<API.Dtos.Admin.OrderToReturnDto>(order));
        }
        
        [HttpGet]
        public async Task<ActionResult<Pagination<API.Dtos.Admin.OrderToReturnDto>>> GetOrders([FromQuery] OrderSpecParams orderParams)
        {
            var spec = new OrderWithFiltersSpecification(orderParams);
            
            var countSpec = new OrderWithFiltersForCountSpecification(orderParams);

            var orders = await _unitOfWork.Repository<Order>().ListAsync(spec);

            var totalItems = await _unitOfWork.Repository<Order>().CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<API.Dtos.Admin.OrderToReturnDto>>(orders);

            return Ok(new Pagination<API.Dtos.Admin.OrderToReturnDto>(orderParams.PageIndex, orderParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<API.Dtos.Admin.OrderToReturnDto>> GetOrder(int id)
        {
            var spec = new OrderByIdSpecification(id);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<API.Dtos.Admin.OrderToReturnDto>(order));
        }
    }
}