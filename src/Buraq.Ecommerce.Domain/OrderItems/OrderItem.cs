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
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public OrderItem(
            int id,
            Guid orderId,
            Guid productId,
            int quantity
        ) : base(id)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

}
