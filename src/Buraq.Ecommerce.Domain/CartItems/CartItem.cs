using Buraq.Ecommerce.Carts;
using Buraq.Ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.CartItems
{
    public class CartItem : FullAuditedAggregateRoot<int>
    {
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(
            int id,
            int cartId,
            int productId,
            int quantity
        ) : base(id)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

}
