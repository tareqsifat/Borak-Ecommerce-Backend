using Buraq.Ecommerce.Addresses;
using Buraq.Ecommerce.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.Customers
{
    public class Customer : FullAuditedAggregateRoot<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public Customer(
            int id,
            string name,
            string email,
            string phone,
            int addressId
        ) : base(id)
        {
            Name = name;
            Email = email;
            Phone = phone;
            AddressId = addressId;
        }
    }

}
