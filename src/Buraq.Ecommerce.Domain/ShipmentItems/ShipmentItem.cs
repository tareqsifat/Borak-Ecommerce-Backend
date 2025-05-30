using Buraq.Ecommerce.OrderItems;
using Buraq.Ecommerce.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.ShipmentItems
{
    public class ShipmentItem : FullAuditedAggregateRoot<int>
    {
        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
        public int Quantity { get; set; }
    }
}
