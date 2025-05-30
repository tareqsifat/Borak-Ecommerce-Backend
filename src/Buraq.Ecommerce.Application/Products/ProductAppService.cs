using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Products
{
    public class ProductAppService :
    CrudAppService<Product, ProductDto, int, PagedAndSortedResultRequestDto, ProductDto, CreateUpdateProductDto>,
    IProductAppService
    {
        public ProductAppService(
        IRepository<Product, int> repository) : base(repository)
        {

        }
    }
}
