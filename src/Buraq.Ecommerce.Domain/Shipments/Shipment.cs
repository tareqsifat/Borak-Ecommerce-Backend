using Buraq.Ecommerce.Enums;
using Buraq.Ecommerce.Orders;
using Buraq.Ecommerce.ShipmentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.Shipments
{
    public class Shipment : FullAuditedAggregateRoot<int>
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public ShipmentStatus Status { get; set; }
        public ICollection<ShipmentItem> Items { get; set; }

        public Shipment(
            int id,
            int orderId,
            DateTime shipmentDate,
            string carrier,
            string trackingNumber,
            ShipmentStatus status
        ) : base(id)
        {
            OrderId = orderId;
            ShipmentDate = shipmentDate;
            Carrier = carrier;
            TrackingNumber = trackingNumber;
            Status = status;
        }
    }

}
