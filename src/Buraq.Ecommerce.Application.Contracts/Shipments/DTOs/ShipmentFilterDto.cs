using Buraq.Ecommerce.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Buraq.Ecommerce.Shipments.DTOs
{
    public class ShipmentFilterDto : PagedAndSortedResultRequestDto
    {
        public int? OrderId { get; set; }
        public ShipmentStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TrackingNumber { get; set; }
    }
}
