using Buraq.Ecommerce.Orders.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.Orders.Interfaces
{
    public interface IOrderAppService : IApplicationService
    {
        Task<OrderDto> GetAsync(int id);
        Task<OrderWithDetailsDto> GetWithDetailsAsync(int id);
        Task<PagedResultDto<OrderDto>> GetListAsync(OrderFilterDto input);
        Task<List<OrderDto>> GetByCustomerAsync(int customerId);
        Task<OrderDto> CreateAsync(CreateOrderDto input);
        Task<OrderDto> UpdateAsync(int id, UpdateOrderDto input);
        Task CancelOrderAsync(int id);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
        Task<OrderStatusDto> GetOrderStatusAsync(int orderId);
        Task<OrderSummaryDto> GetOrderSummaryAsync(int orderId);
    }
}
