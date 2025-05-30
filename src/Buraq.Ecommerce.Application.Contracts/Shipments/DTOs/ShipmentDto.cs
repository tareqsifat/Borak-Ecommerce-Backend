using Buraq.Ecommerce.Enums;
using Buraq.Ecommerce.Orders.DTOs;
using Buraq.Ecommerce.ShipmentItems.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Buraq.Ecommerce.Shipments.DTOs
{
    public class ShipmentDto : FullAuditedEntityDto<int>
    {
        public int OrderId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public ShipmentStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
