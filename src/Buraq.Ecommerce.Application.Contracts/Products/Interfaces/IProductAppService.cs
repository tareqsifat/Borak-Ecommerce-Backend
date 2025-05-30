using Buraq.Ecommerce.Products.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.Products.Interfaces
{
    public interface IProductAppService : IApplicationService
    {
        Task<ProductDto> GetAsync(int id);
        Task<ProductWithDetailsDto> GetWithDetailsAsync(int id);
        Task<PagedResultDto<ProductDto>> GetListAsync(ProductFilterDto input);
        Task<List<ProductLookupDto>> GetActiveProductsAsync();
        Task<ProductDto> CreateAsync(CreateUpdateProductDto input);
        Task<ProductDto> UpdateAsync(int id, CreateUpdateProductDto input);
        Task ToggleStatusAsync(int id);
        Task UpdateStockAsync(int id, int quantityChange);
        Task DeleteAsync(int id);
    }
}
