using Buraq.Ecommerce.OrderItems.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.OrderItems.Interfaces
{
    public interface IOrderItemAppService : IApplicationService
    {
        Task<OrderItemDto> GetAsync(int id);
        Task<List<OrderItemDto>> GetByOrderIdAsync(int orderId);
        Task<PagedResultDto<OrderItemDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<OrderItemDto> CreateAsync(CreateUpdateOrderItemDto input);
        Task<OrderItemDto> UpdateAsync(int id, CreateUpdateOrderItemDto input);
        Task DeleteAsync(int id);
        Task DeleteByOrderAsync(int orderId);
        Task<int> GetCountForOrderAsync(int orderId);
        Task<decimal> CalculateSubtotalAsync(int orderItemId);
    }
}
