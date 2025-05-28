using Buraq.Ecommerce.Orders;
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
        public string ShipmentStatus { get; set; }

        public Shipment(
            int id,
            int orderId,
            DateTime shipmentDate,
            string carrier,
            string trackingNumber,
            string shipmentStatus
        ) : base(id)
        {
            OrderId = orderId;
            ShipmentDate = shipmentDate;
            Carrier = carrier;
            TrackingNumber = trackingNumber;
            ShipmentStatus = shipmentStatus;
        }
    }

}
