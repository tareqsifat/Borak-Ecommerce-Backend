using Buraq.Ecommerce.ShipmentItems.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Shipments.DTOs
{
    public class ShipmentWithItemsDto : ShipmentDto
    {
        public List<ShipmentItemDto> Items { get; set; } = new List<ShipmentItemDto>();
    }
}
