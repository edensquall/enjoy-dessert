using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersAndAllowShowingForCountSpecificication : BaseSpecification<Product>
    {
        public ProductWithFiltersAndAllowShowingForCountSpecificication(ProductSpecParams productParams)
            : base(x =>
                (x.IsShow && !x.IsShowByDate || (DateTime.Now >= x.StartDate && (x.EndDate == null || DateTime.Now <= x.EndDate))) &&
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {

        }
    }
}