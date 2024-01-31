using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Admin
{
    public class NewsToReturnDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Caption { get; set; }
        public string? Content { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsShow { get; set; }
        public bool IsShowByDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}