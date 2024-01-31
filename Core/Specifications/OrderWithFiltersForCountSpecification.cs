using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderWithFiltersForCountSpecification : BaseSpecification<Order>
    {
        public OrderWithFiltersForCountSpecification(OrderSpecParams orderParams)
            : base(x =>
                (string.IsNullOrEmpty(orderParams.Search) || x.BuyerUserName.ToLower().Contains(orderParams.Search)))
        {
        } 
    }
}