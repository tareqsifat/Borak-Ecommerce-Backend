using Buraq.Ecommerce.Categories.DTOs;
using Buraq.Ecommerce.Suppliers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Products.DTOs
{
    public class ProductWithDetailsDto : ProductDto
    {
        public CategoryDto Category { get; set; }
        public SupplierDto Supplier { get; set; }
    }

}
