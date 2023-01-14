using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string? firstName, string? lastName, string? phoneNumber, string? city, string? county, string? street, string? zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            County = county;
            City = city;
            Street = street;
            ZipCode = zipCode;
            PhoneNumber = phoneNumber;
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? County { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}