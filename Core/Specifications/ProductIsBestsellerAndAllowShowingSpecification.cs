using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductIsBestsellerAndAllowShowingSpecification : BaseSpecification<Product>
    {
        public ProductIsBestsellerAndAllowShowingSpecification() :
            base(x =>
            (x.IsShow && !x.IsShowByDate || (DateTime.Now >= x.StartDate && (x.EndDate == null || DateTime.Now <= x.EndDate))) &&
            x.IsBestseller == true)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductImages);
        }
    }
}