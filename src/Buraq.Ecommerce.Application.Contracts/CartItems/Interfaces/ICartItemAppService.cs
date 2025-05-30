using Buraq.Ecommerce.CartItems.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.CartItems.Interfaces
{
    public interface ICartItemAppService : IApplicationService
    {
        Task<CartItemDto> GetAsync(int id);
        Task<PagedResultDto<CartItemDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<CartItemDto>> GetItemsByCartIdAsync(int cartId);
        Task<CartItemDto> CreateAsync(CreateUpdateCartItemDto input);
        Task<CartItemDto> UpdateAsync(int id, CreateUpdateCartItemDto input);
        Task UpdateQuantityAsync(int id, int newQuantity);
        Task DeleteAsync(int id);
        Task ClearCartAsync(int cartId);
    }

}
