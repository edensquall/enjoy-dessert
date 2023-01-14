using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class News : BaseEntity
    {
        public string? Title { get; set; }
        public string? Caption { get; set; }
        public string? Content { get; set; }
        public string? Thumbnail { get; set; }
        public bool IsShow { get; set; } = true;
        public bool IsShowByDate { get; set; } = false;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}