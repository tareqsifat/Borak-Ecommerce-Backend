using Buraq.Ecommerce.Enums;
using Buraq.Ecommerce.Orders.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Shipments.DTOs
{
    public class ShipmentDto
    {
        public int OrderId { get; set; }
        public OrderDto Order { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public ShipmentStatus Status { get; set; }
        public List<ShipmentItemDto> Items { get; set; }
    }
}
