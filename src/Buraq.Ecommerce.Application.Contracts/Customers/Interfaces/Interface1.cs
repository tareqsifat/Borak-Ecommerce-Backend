using Buraq.Ecommerce.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.Customers.Interfaces
{
    public interface ICustomerAppService : IApplicationService
    {
        Task<CustomerDto> GetAsync(int id);
        Task<CustomerWithDetailsDto> GetWithDetailsAsync(int id);
        Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerListDto input);
        Task<List<CustomerLookupDto>> GetLookupListAsync();
        Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input);
        Task<CustomerDto> UpdateAsync(int id, CreateUpdateCustomerDto input);
        Task DeleteAsync(int id);
        Task<int> GetCartCountAsync(int customerId);
    }
}
