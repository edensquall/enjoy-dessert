using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class NewsWithFiltersForCountSpecificication : BaseSpecification<News>
    {
        public NewsWithFiltersForCountSpecificication(NewsSpecParams newsParams)
            : base(x =>
                (x.IsShow && !x.IsShowByDate || (DateTime.Now >= x.StartDate && (x.EndDate == null || DateTime.Now <= x.EndDate)))
            )
        {
        }
    }
}