using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Orders.DTOs
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public int ShippingAddressId { get; set; }
        public List<OrderItemCreateDto> Items { get; set; } = new();
    }
}
