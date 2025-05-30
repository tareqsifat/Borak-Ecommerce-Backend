using Buraq.Ecommerce.Addresses.DTOs;
using Buraq.Ecommerce.Carts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Customers.DTOs
{
    public class CustomerWithDetailsDto : CustomerDto
    {
        public AddressDto Address { get; set; }
        public List<CartDto> Carts { get; set; } = new List<CartDto>();
    }
}
