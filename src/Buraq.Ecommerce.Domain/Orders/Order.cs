using Buraq.Ecommerce.Addresses;
using Buraq.Ecommerce.Customers;
using Buraq.Ecommerce.OrderItems;
using Buraq.Ecommerce.Payments;
using Buraq.Ecommerce.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.Orders
{
    public class Order : FullAuditedAggregateRoot<int>
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public int ShippingAddressId { get; set; }
        public Address ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
        public ICollection<Payment> Payments { get; set; }

        public Order(
            int id,
            int customerId,
            DateTime orderDate,
            int shippingAddressId,
            decimal totalAmount
        ) : base(id)
        {
            CustomerId = customerId;
            OrderDate = orderDate;
            ShippingAddressId = shippingAddressId;
            TotalAmount = totalAmount;
            OrderItems = new List<OrderItem>();
            Shipments = new List<Shipment>();
            Payments = new List<Payment>();
        }
    }

}
