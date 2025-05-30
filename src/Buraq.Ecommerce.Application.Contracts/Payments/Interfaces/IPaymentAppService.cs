using Buraq.Ecommerce.Payments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.Payments.Interfaces
{
    public interface IPaymentAppService : IApplicationService
    {
        Task<PaymentDto> GetAsync(int id);
        Task<List<PaymentDto>> GetByOrderAsync(int orderId);
        Task<PagedResultDto<PaymentDto>> GetListAsync(PaymentFilterDto input);
        Task<PaymentDto> CreateAsync(CreatePaymentDto input);
        Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto input);
        Task ProcessPaymentAsync(int id);
        Task RefundPaymentAsync(int id, decimal amount);
        Task<PaymentStatusDto> GetPaymentStatusAsync(int id);
        Task DeleteAsync(int id);
    }

}
