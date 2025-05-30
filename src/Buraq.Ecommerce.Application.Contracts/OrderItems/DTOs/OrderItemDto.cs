using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Buraq.Ecommerce.OrderItems.DTOs
{
    public class OrderItemDto : FullAuditedEntityDto<int>
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        // Navigation properties
        public ProductDto Product { get; set; }
        public OrderDto Order { get; set; }
    }
}
