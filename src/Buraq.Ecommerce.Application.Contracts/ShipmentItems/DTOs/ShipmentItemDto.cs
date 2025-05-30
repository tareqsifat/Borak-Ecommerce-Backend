using Buraq.Ecommerce.OrderItems.DTOs;
using Buraq.Ecommerce.Shipments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Buraq.Ecommerce.ShipmentItems.DTOs
{
    public class ShipmentItemDto : FullAuditedEntityDto<int>
    {
        public int ShipmentId { get; set; }
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public ShipmentDto Shipment { get; set; }
        public OrderItemDto OrderItem { get; set; }
    }
}
