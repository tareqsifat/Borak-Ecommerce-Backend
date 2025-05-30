using Buraq.Ecommerce.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Shipments.DTOs
{
    public class ShipmentSummaryDto
    {
        public int ShipmentId { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public ShipmentStatus Status { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public int ItemCount { get; set; }
    }
}
