using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Slide : BaseEntity
    {
        public string? Image { get; set; }
        public string? Url { get; set; }
        public bool IsShow { get; set; } = true;
    }
}