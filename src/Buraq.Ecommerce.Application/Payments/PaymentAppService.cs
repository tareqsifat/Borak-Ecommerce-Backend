using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.Enums;
using Buraq.Ecommerce.Orders;
using Buraq.Ecommerce.Payments.DTOs;
using Buraq.Ecommerce.Payments.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Payments
{
    public class PaymentAppService : ApplicationService, IPaymentAppService
    {
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IRepository<Order, int> _orderRepository;

        public PaymentAppService(
            IRepository<Payment, int> paymentRepository,
            IRepository<Order, int> orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentDto> GetAsync(int id)
        {
            var payment = await _paymentRepository.GetAsync(id);
            return ObjectMapper.Map<Payment, PaymentDto>(payment);
        }

        public async Task<List<PaymentDto>> GetByOrderAsync(int orderId)
        {
            var payments = await _paymentRepository.GetListAsync(p => p.OrderId == orderId);
            return ObjectMapper.Map<List<Payment>, List<PaymentDto>>(payments);
        }

        public async Task<PagedResultDto<PaymentDto>> GetListAsync(PaymentFilterDto input)
        {
            var queryable = await _paymentRepository.GetQueryableAsync();

            if (input.OrderId.HasValue)
            {
                queryable = queryable.Where(p => p.OrderId == input.OrderId.Value);
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(p => p.Status == input.Status.Value);
            }

            if (input.StartDate.HasValue)
            {
                queryable = queryable.Where(p => p.PaymentDate >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                queryable = queryable.Where(p => p.PaymentDate <= input.EndDate.Value);
            }

            var query = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(p => p.PaymentDate);

            var payments = await AsyncExecuter.ToListAsync(query);
            var totalCount = await _paymentRepository.GetCountAsync();

            return new PagedResultDto<PaymentDto>(
                totalCount,
                ObjectMapper.Map<List<Payment>, List<PaymentDto>>(payments)
            );
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto input)
        {
            if (!await _orderRepository.AnyAsync(o => o.Id == input.OrderId))
            {
                throw new UserFriendlyException("Order not found!");
            }

            var payment = new Payment(
                id: 0, 
                orderId: input.OrderId,
                paymentDate: DateTime.Now,
                amount: input.Amount,
                paymentMethod: input.PaymentMethod,
                Status: PaymentStatus.Pending
            );

            payment = await _paymentRepository.InsertAsync(payment);
            return ObjectMapper.Map<Payment, PaymentDto>(payment);
        }

        public async Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto input)
        {
            var payment = await _paymentRepository.GetAsync(id);

            if (input.PaymentMethod != null)
            {
                payment.PaymentMethod = input.PaymentMethod;
            }

            payment = await _paymentRepository.UpdateAsync(payment);
            return ObjectMapper.Map<Payment, PaymentDto>(payment);
        }

        public async Task ProcessPaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetAsync(id);

            payment.Status = PaymentStatus.Completed;
            payment.PaymentDate = DateTime.Now;

            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task RefundPaymentAsync(int id, decimal amount)
        {
            var payment = await _paymentRepository.GetAsync(id);

            if (payment.Status != PaymentStatus.Completed)
            {
                throw new UserFriendlyException("Only completed payments can be refunded!");
            }

            if (amount <= 0 || amount > payment.Amount)
            {
                throw new UserFriendlyException("Invalid refund amount!");
            }

            if (amount == payment.Amount)
            {
                payment.Status = PaymentStatus.Refunded;
            }
            else
            {
                payment.Status = PaymentStatus.PartiallyRefunded;
            }

            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task<PaymentStatusDto> GetPaymentStatusAsync(int id)
        {
            var payment = await _paymentRepository.GetAsync(id);
            return new PaymentStatusDto
            {
                PaymentId = id,
                Status = payment.Status,
                AmountPaid = payment.Status == PaymentStatus.Completed ? payment.Amount : 0,
                CanRefund = payment.Status == PaymentStatus.Completed
            };
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _paymentRepository.GetAsync(id);

            if (payment.Status == PaymentStatus.Completed ||
                payment.Status == PaymentStatus.Refunded)
            {
                throw new UserFriendlyException("Cannot delete processed payments!");
            }

            await _paymentRepository.DeleteAsync(id);
        }
    }
}
