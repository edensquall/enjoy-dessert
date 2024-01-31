using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Admin
{
    public class UserToReturnDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Grade { get; set; }
        public bool? IsAdmin { get; set; }
    }
}