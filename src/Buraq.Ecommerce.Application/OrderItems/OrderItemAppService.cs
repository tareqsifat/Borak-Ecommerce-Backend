using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.OrderItems.DTOs;
using Buraq.Ecommerce.OrderItems.Interfaces;
using Buraq.Ecommerce.Orders;
using Buraq.Ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.OrderItems
{
    public class OrderItemAppService : ApplicationService, IOrderItemAppService
    {
        private readonly IRepository<OrderItem, int> _orderItemRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Order, int> _orderRepository;

        public OrderItemAppService(
            IRepository<OrderItem, int> orderItemRepository,
            IRepository<Product, int> productRepository,
            IRepository<Order, int> orderRepository)
        {
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<OrderItemDto> GetAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetAsync(id);
            return ObjectMapper.Map<OrderItem, OrderItemDto>(orderItem);
        }

        public async Task<List<OrderItemDto>> GetByOrderIdAsync(int orderId)
        {
            var queryable = await _orderItemRepository.GetQueryableAsync();
            var orderItems = await AsyncExecuter.ToListAsync(
                queryable.Where(oi => oi.OrderId == orderId)
            );

            return ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(orderItems);
        }

        public async Task<PagedResultDto<OrderItemDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _orderItemRepository.GetQueryableAsync();
            var query = queryable.Skip(input.SkipCount)
                                .Take(input.MaxResultCount);

            if (!string.IsNullOrEmpty(input.Sorting))
            {
                query = input.Sorting switch
                {
                    "quantity asc" => query.OrderBy(oi => oi.Quantity),
                    "quantity desc" => query.OrderByDescending(oi => oi.Quantity),
                    _ => query.OrderBy(oi => oi.CreationTime)
                };
            }

            var orderItems = await AsyncExecuter.ToListAsync(query);
            var totalCount = await _orderItemRepository.GetCountAsync();

            return new PagedResultDto<OrderItemDto>(
                totalCount,
                ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(orderItems)
            );
        }

        public async Task<OrderItemDto> CreateAsync(CreateUpdateOrderItemDto input)
        {
            if (!await _orderRepository.AnyAsync(o => o.Id == input.OrderId))
            {
                throw new UserFriendlyException("Order not found!");
            }

            if (!await _productRepository.AnyAsync(p => p.Id == input.ProductId))
            {
                throw new UserFriendlyException("Product not found!");
            }

            if (input.Quantity <= 0)
            {
                throw new UserFriendlyException("Quantity must be greater than zero!");
            }

            var orderItem = new OrderItem(
                id: 0, 
                orderId: input.OrderId,
                productId: input.ProductId,
                quantity: input.Quantity
            );

            orderItem = await _orderItemRepository.InsertAsync(orderItem);
            return ObjectMapper.Map<OrderItem, OrderItemDto>(orderItem);
        }

        public async Task<OrderItemDto> UpdateAsync(int id, CreateUpdateOrderItemDto input)
        {
            var orderItem = await _orderItemRepository.GetAsync(id);

            if (orderItem.ProductId != input.ProductId &&
                !await _productRepository.AnyAsync(p => p.Id == input.ProductId))
            {
                throw new UserFriendlyException("Product not found!");
            }

            if (input.Quantity <= 0)
            {
                throw new UserFriendlyException("Quantity must be greater than zero!");
            }

            orderItem.ProductId = input.ProductId;
            orderItem.Quantity = input.Quantity;

            orderItem = await _orderItemRepository.UpdateAsync(orderItem);
            return ObjectMapper.Map<OrderItem, OrderItemDto>(orderItem);
        }

        public async Task DeleteAsync(int id)
        {
            await _orderItemRepository.DeleteAsync(id);
        }

        public async Task DeleteByOrderAsync(int orderId)
        {
            await _orderItemRepository.DeleteAsync(oi => oi.OrderId == orderId);
        }

        public async Task<int> GetCountForOrderAsync(int orderId)
        {
            return await _orderItemRepository.CountAsync(oi => oi.OrderId == orderId);
        }

        public async Task<decimal> CalculateSubtotalAsync(int orderItemId)
        {
            var orderItem = await _orderItemRepository.GetAsync(orderItemId);
            var product = await _productRepository.GetAsync(orderItem.ProductId);

            return product.Price * orderItem.Quantity;
        }
    }
}
