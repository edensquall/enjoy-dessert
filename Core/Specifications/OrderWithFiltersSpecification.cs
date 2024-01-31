using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderWithFiltersSpecification : BaseSpecification<Order>
    {
        public OrderWithFiltersSpecification(OrderSpecParams orderParams)
            : base(x =>
                (string.IsNullOrEmpty(orderParams.Search) || x.BuyerUserName.ToLower().Contains(orderParams.Search)))
        {
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }
    }
}