using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(string userName, OrderSpecParams orderParams)
            : base(o => o.BuyerUserName == userName)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecification(int id, string userName)
            : base(o => o.Id == id && o.BuyerUserName == userName)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}