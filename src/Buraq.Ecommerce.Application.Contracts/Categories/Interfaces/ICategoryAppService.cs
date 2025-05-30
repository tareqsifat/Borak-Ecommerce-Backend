using Buraq.Ecommerce.Categories.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.Categories.Interfaces
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<CategoryDto> GetAsync(int id);
        Task<CategoryWithProductsDto> GetWithProductsAsync(int id);
        Task<PagedResultDto<CategoryDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<CategoryLookupDto>> GetLookupListAsync();
        Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input);
        Task<CategoryDto> UpdateAsync(int id, CreateUpdateCategoryDto input);
        Task DeleteAsync(int id);
        Task<int> GetProductCountAsync(int categoryId);
    }
}
