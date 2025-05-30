using Buraq.Ecommerce.Addresses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.Addresses.Interfaces
{
    public interface IAddressAppService : IApplicationService
    {
        Task<AddressDto> GetAsync(int id);
        Task<List<AddressDto>> GetAllAsync();
        Task<AddressDto> CreateAsync(CreateUpdateAddressDto input);
        Task<AddressDto> UpdateAsync(int id, CreateUpdateAddressDto input);
        Task DeleteAsync(int id);
    }
}
