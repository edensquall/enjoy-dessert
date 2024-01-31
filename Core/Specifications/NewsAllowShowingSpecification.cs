using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class NewsAllowShowingSpecification : BaseSpecification<News>
    {
        public NewsAllowShowingSpecification()
            : base(x =>
                (x.IsShow && !x.IsShowByDate || (DateTime.Now >= x.StartDate && (x.EndDate == null || DateTime.Now <= x.EndDate)))
            )
        {

        }
        
        public NewsAllowShowingSpecification(NewsSpecParams newsParams)
            : base(x =>
                (x.IsShow && !x.IsShowByDate || (DateTime.Now >= x.StartDate && (x.EndDate == null || DateTime.Now <= x.EndDate)))
            )
        {
            ApplyPaging(newsParams.PageSize * (newsParams.PageIndex - 1), newsParams.PageSize);
            AddOrderByDescending(x => x.StartDate);
        }

        public NewsAllowShowingSpecification(int id)
            : base(x =>
            (x.IsShow && !x.IsShowByDate || (DateTime.Now >= x.StartDate && (x.EndDate == null || DateTime.Now <= x.EndDate))) &&
            x.Id == id)
        {
        }
    }
}