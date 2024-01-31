using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderByBuyerUserNameSpecification : BaseSpecification<Order>
    {
        public OrderByBuyerUserNameSpecification(string userName)
            : base(o => o.BuyerUserName == userName)
        {
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrderByBuyerUserNameSpecification(string userName, OrderSpecParams orderParams)
            : base(o => o.BuyerUserName == userName)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
            AddOrderByDescending(o => o.OrderDate);
        }
    }
}