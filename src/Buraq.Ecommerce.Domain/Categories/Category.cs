using Buraq.Ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.Categories
{
    public class Category : FullAuditedAggregateRoot<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public Category(int id, string name, string description, string image) : base(id)
        {
            Name = name;
            Description = description;
            Image = image;
            Products = new List<Product>();
        }
    }
}
