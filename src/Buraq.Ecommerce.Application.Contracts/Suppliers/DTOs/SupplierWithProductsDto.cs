using Buraq.Ecommerce.Products.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Suppliers.DTOs
{
    public class SupplierWithProductsDto : SupplierDto
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
