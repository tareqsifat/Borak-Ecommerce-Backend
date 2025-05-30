using Buraq.Ecommerce.Suppliers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.Suppliers.Interfaces
{
    public interface ISupplierAppService : IApplicationService
    {
        Task<SupplierDto> GetAsync(int id);
        Task<SupplierWithProductsDto> GetWithProductsAsync(int id);
        Task<PagedResultDto<SupplierDto>> GetListAsync(SupplierFilterDto input);
        Task<List<SupplierLookupDto>> GetLookupListAsync();
        Task<SupplierDto> CreateAsync(CreateUpdateSupplierDto input);
        Task<SupplierDto> UpdateAsync(int id, CreateUpdateSupplierDto input);
        Task DeleteAsync(int id);
        Task<int> GetProductCountAsync(int supplierId);
    }
}
