using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace API.Dtos.Admin
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Status { get; set; } 
    }
}