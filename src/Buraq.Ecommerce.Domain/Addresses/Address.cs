using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.Addresses
{
    public class Address : FullAuditedAggregateRoot<int>
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        public Address(
            int id,
            string name,
            string street,
            string country
        ) : base(id)
        {
            Name = name;
            Street = street;
            Country = country;
        }
    }
}
