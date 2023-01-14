using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int productItemId, string? productName, string? imageUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            ImageUrl = imageUrl;
        }

        public int ProductItemId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
    }
}