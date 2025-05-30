using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Orders.DTOs
{
    public class OrderStatusDto
    {
        public int OrderId { get; set; }
        public string PaymentStatus { get; set; }
        public string FulfillmentStatus { get; set; }
    }
}
