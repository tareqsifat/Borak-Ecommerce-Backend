using Buraq.Ecommerce.Carts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.Carts.Interfaces
{
    public interface ICartAppService : IApplicationService
    {
        Task<CartDto> GetAsync(int id);
        Task<CartWithItemsDto> GetWithItemsAsync(int id);
        Task<CartDto> GetByCustomerAsync(int customerId);
        Task<CartWithItemsDto> GetByCustomerWithItemsAsync(int customerId);
        Task<PagedResultDto<CartDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<CartDto> CreateAsync(CreateOrUpdateCartDto input);
        Task<CartDto> UpdateAsync(int id, CreateOrUpdateCartDto input);
        Task DeleteAsync(int id);
        Task<int> GetItemCountAsync(int cartId);
        Task<decimal> CalculateTotalAsync(int cartId);
    }
}
