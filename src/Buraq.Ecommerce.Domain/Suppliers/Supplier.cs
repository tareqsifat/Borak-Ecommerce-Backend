using Buraq.Ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.Suppliers
{
    public class Supplier : FullAuditedAggregateRoot<int>
    {
        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public Supplier(int id, string name, string contactEmail, string phoneNumber) : base(id)
        {
            Name = name;
            ContactEmail = contactEmail;
            PhoneNumber = phoneNumber;
            Products = new List<Product>();
        }
    }
}
