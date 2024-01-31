using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductImage:BaseEntity
    {
        public ProductImage()
        {

        }

        public ProductImage(string name, int order)
        {
            Name = name;
            Order = order;
        }

        public string? Name { get; set; }
        public int Order { get; set; }
        public Product? Product { get; set; }
        public int ProductId { get; set; }
    }
}