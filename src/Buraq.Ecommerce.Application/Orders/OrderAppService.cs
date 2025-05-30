using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.Addresses;
using Buraq.Ecommerce.Customers;
using Buraq.Ecommerce.Enums;
using Buraq.Ecommerce.OrderItems;
using Buraq.Ecommerce.Orders.DTOs;
using Buraq.Ecommerce.Orders.Interfaces;
using Buraq.Ecommerce.Payments;
using Buraq.Ecommerce.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Orders
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Order, int> _orderRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<Address, int> _addressRepository;
        private readonly IRepository<OrderItem, int> _orderItemRepository;
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IRepository<Shipment, int> _shipmentRepository;

        public OrderAppService(
            IRepository<Order, int> orderRepository,
            IRepository<Customer, int> customerRepository,
            IRepository<Address, int> addressRepository,
            IRepository<OrderItem, int> orderItemRepository,
            IRepository<Payment, int> paymentRepository,
            IRepository<Shipment, int> shipmentRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _orderItemRepository = orderItemRepository;
            _paymentRepository = paymentRepository;
            _shipmentRepository = shipmentRepository;
        }

        public async Task<OrderDto> GetAsync(int id)
        {
            var order = await _orderRepository.GetAsync(id);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<OrderWithDetailsDto> GetWithDetailsAsync(int id)
        {
            var order = await _orderRepository.GetAsync(id, includeDetails: true);

            await _orderRepository.EnsurePropertyLoadedAsync(order, o => o.Customer);
            await _orderRepository.EnsurePropertyLoadedAsync(order, o => o.ShippingAddress);
            await _orderRepository.EnsureCollectionLoadedAsync(order, o => o.OrderItems);
            await _orderRepository.EnsureCollectionLoadedAsync(order, o => o.Payments);
            await _orderRepository.EnsureCollectionLoadedAsync(order, o => o.Shipments);

            var dto = ObjectMapper.Map<Order, OrderWithDetailsDto>(order);
            dto.AmountPaid = await CalculateAmountPaidAsync(id);
            dto.RemainingBalance = order.TotalAmount - dto.AmountPaid;

            return dto;
        }

        public async Task<PagedResultDto<OrderDto>> GetListAsync(OrderFilterDto input)
        {
            var queryable = await _orderRepository.GetQueryableAsync();

            if (input.CustomerId.HasValue)
            {
                queryable = queryable.Where(o => o.CustomerId == input.CustomerId.Value);
            }

            if (input.StartDate.HasValue)
            {
                queryable = queryable.Where(o => o.OrderDate >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                queryable = queryable.Where(o => o.OrderDate <= input.EndDate.Value);
            }

            if (input.MinAmount.HasValue)
            {
                queryable = queryable.Where(o => o.TotalAmount >= input.MinAmount.Value);
            }

            if (input.MaxAmount.HasValue)
            {
                queryable = queryable.Where(o => o.TotalAmount <= input.MaxAmount.Value);
            }

            var query = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            query = input.Sorting switch
            {
                "date asc" => query.OrderBy(o => o.OrderDate),
                "date desc" => query.OrderByDescending(o => o.OrderDate),
                "amount asc" => query.OrderBy(o => o.TotalAmount),
                "amount desc" => query.OrderByDescending(o => o.TotalAmount),
                _ => query.OrderByDescending(o => o.OrderDate)
            };

            var orders = await AsyncExecuter.ToListAsync(query);
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            return new PagedResultDto<OrderDto>(
                totalCount,
                ObjectMapper.Map<List<Order>, List<OrderDto>>(orders)
            );
        }

        public async Task<List<OrderDto>> GetByCustomerAsync(int customerId)
        {
            var queryable = await _orderRepository.GetQueryableAsync();
            var orders = await AsyncExecuter.ToListAsync(
                queryable.Where(o => o.CustomerId == customerId)
                    .OrderByDescending(o => o.OrderDate)
            );

            return ObjectMapper.Map<List<Order>, List<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            if (!await _customerRepository.AnyAsync(c => c.Id == input.CustomerId))
            {
                throw new UserFriendlyException("Customer not found!");
            }

            if (!await _addressRepository.AnyAsync(a => a.Id == input.ShippingAddressId))
            {
                throw new UserFriendlyException("Shipping address not found!");
            }

            var order = new Order(
                id: 0,
                customerId: input.CustomerId,
                orderDate: DateTime.Now,
                shippingAddressId: input.ShippingAddressId,
                totalAmount: 0 
            );

            order = await _orderRepository.InsertAsync(order);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<OrderDto> UpdateAsync(int id, UpdateOrderDto input)
        {
            var order = await _orderRepository.GetAsync(id);

            if (input.ShippingAddressId.HasValue)
            {
                if (!await _addressRepository.AnyAsync(a => a.Id == input.ShippingAddressId.Value))
                {
                    throw new UserFriendlyException("Shipping address not found!");
                }
                order.ShippingAddressId = input.ShippingAddressId.Value;
            }

            order = await _orderRepository.UpdateAsync(order);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task CancelOrderAsync(int id)
        {
            var order = await _orderRepository.GetAsync(id);

            var shipments = await _shipmentRepository.GetListAsync(s => s.OrderId == id);

            if (shipments.Any(s => s.Status == ShipmentStatus.Shipped || s.Status == ShipmentStatus.Delivered))
            {
                throw new UserFriendlyException("Order cannot be cancelled because it has shipped items!");
            }
            
            order.Status = OrderStatus.Cancelled;

            await _orderRepository.UpdateAsync(order);
        }

        public async Task<decimal> CalculateOrderTotalAsync(int orderId)
        {
            var orderItems = await _orderItemRepository.GetListAsync(oi => oi.OrderId == orderId);
            decimal total = 0;

            foreach (var item in orderItems)
            {
                total += item.Quantity * item.UnitPrice;
            }

            return total;
        }

        public async Task<OrderStatusDto> GetOrderStatusAsync(int orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            var payments = await _paymentRepository.GetListAsync(p => p.OrderId == orderId);
            var shipments = await _shipmentRepository.GetListAsync(s => s.OrderId == orderId);

            return new OrderStatusDto
            {
                OrderId = orderId,
                PaymentStatus = CalculatePaymentStatus(payments, order.TotalAmount),
                FulfillmentStatus = CalculateFulfillmentStatus(shipments, order.OrderItems.Count)
            };
        }

        public async Task<OrderSummaryDto> GetOrderSummaryAsync(int orderId)
        {
            var order = await GetWithDetailsAsync(orderId);

            return new OrderSummaryDto
            {
                OrderId = orderId,
                CustomerName = order.Customer?.Name,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                AmountPaid = order.AmountPaid,
                RemainingBalance = order.RemainingBalance,
                ItemCount = order.OrderItems.Count,
                ShipmentCount = order.Shipments.Count
            };
        }

        private async Task<decimal> CalculateAmountPaidAsync(int orderId)
        {
            var payments = await _paymentRepository.GetListAsync(p => p.OrderId == orderId && p.Status == PaymentStatus.Completed);
            return payments.Sum(p => p.Amount);
        }

        private string CalculatePaymentStatus(List<Payment> payments, decimal orderTotal)
        {
            var amountPaid = payments.Where(p => p.Status == PaymentStatus.Completed).Sum(p => p.Amount);

            if (amountPaid >= orderTotal) return "Paid in Full";
            if (amountPaid > 0) return "Partially Paid";
            return "Unpaid";
        }

        private string CalculateFulfillmentStatus(List<Shipment> shipments, int totalItems)
        {
            if (shipments.Count == 0) return "Unfulfilled";

            var shippedItems = shipments.Sum(s => s.Items.Count);

            if (shippedItems >= totalItems) return "Fully Shipped";
            return "Partially Shipped";
        }
    }
}
