using Buraq.Ecommerce.Categories;
using Buraq.Ecommerce.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.Products
{
    public class Product : FullAuditedAggregateRoot<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public int Image { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Supplier? Supplier { get; set; }

        public Product(
            int id,
            string name,
            string description,
            decimal price,
            int stockQuantity,
            int categoryId,
            int supplierId,
            bool isActive = true
        ) : base(id)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            CategoryId = categoryId;
            SupplierId = supplierId;
            IsActive = isActive;
        }
    }
}
