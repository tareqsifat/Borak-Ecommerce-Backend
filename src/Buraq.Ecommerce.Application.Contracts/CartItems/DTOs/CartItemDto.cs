using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Buraq.Ecommerce.CartItems.DTOs
{
    public class CartItemDto : FullAuditedEntityDto<int>
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        //public ProductDto Product { get; set; }
        //public CartDto Cart { get; set; }
    }
}
