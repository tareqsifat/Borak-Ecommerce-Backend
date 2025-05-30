using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Buraq.Ecommerce.Products
{
    public class ProductDto : EntityDto<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public int Image { get; set; }
        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }
    }
}
