using Buraq.Ecommerce.Addresses.DTOs;
using Buraq.Ecommerce.Customers.DTOs;
using Buraq.Ecommerce.OrderItems.DTOs;
using Buraq.Ecommerce.Payments.DTOs;
using Buraq.Ecommerce.Shipments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Orders.DTOs
{
    public class OrderWithDetailsDto : OrderDto
    {
        public CustomerDto Customer { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public List<PaymentDto> Payments { get; set; } 
        public List<ShipmentDto> Shipments { get; set; } 
        public decimal AmountPaid { get; set; }
        public decimal RemainingBalance { get; set; }
    }
}
