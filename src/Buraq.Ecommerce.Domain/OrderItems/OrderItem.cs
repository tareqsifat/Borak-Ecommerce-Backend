using Buraq.Ecommerce.Orders;
using Buraq.Ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.OrderItems
{
    public class OrderItem : FullAuditedAggregateRoot<int>
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public OrderItem(
            int id,
            int orderId,
            int productId,
            int quantity
        ) : base(id)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

}
