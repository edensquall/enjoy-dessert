using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class NewsSpecification : BaseSpecification<News>
    {
        public NewsSpecification(NewsSpecParams newsParams)
            : base()
        {
            ApplyPaging(newsParams.PageSize * (newsParams.PageIndex - 1), newsParams.PageSize);
            AddOrderByDescending(x => x.StartDate);
        }
    }
}