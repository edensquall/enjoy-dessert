using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product:BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsBestseller { get; set; } = false;
        public bool IsShow { get; set; } = true;
        public bool IsShowByDate { get; set; } = false;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProductType? ProductType { get; set; }
        public int? ProductTypeId { get; set; }
        public List<ProductImage>? ProductImages { get; set; } = new List<ProductImage>();
    }
}