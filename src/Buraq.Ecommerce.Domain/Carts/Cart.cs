using Buraq.Ecommerce.CartItems;
using Buraq.Ecommerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.Carts
{
    public class Cart : FullAuditedAggregateRoot<int>
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

        public Cart(
            int id,
            int customerId
        ) : base(id)
        {
            CustomerId = customerId;
            CartItems = new List<CartItem>();
        }
    }

}
