using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.OrderItems;
using Buraq.Ecommerce.ShipmentItems.DTOs;
using Buraq.Ecommerce.ShipmentItems.Interfaces;
using Buraq.Ecommerce.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.ShipmentItems
{
    public class ShipmentItemAppService : ApplicationService, IShipmentItemAppService
    {
        private readonly IRepository<ShipmentItem, int> _shipmentItemRepository;
        private readonly IRepository<Shipment, int> _shipmentRepository;
        private readonly IRepository<OrderItem, int> _orderItemRepository;

        public ShipmentItemAppService(
            IRepository<ShipmentItem, int> shipmentItemRepository,
            IRepository<Shipment, int> shipmentRepository,
            IRepository<OrderItem, int> orderItemRepository)
        {
            _shipmentItemRepository = shipmentItemRepository;
            _shipmentRepository = shipmentRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<ShipmentItemDto> GetAsync(int id)
        {
            var shipmentItem = await _shipmentItemRepository.GetAsync(id);
            return ObjectMapper.Map<ShipmentItem, ShipmentItemDto>(shipmentItem);
        }

        public async Task<List<ShipmentItemDto>> GetByShipmentAsync(int shipmentId)
        {
            var queryable = await _shipmentItemRepository.GetQueryableAsync();
            var items = await AsyncExecuter.ToListAsync(
                queryable.Where(si => si.ShipmentId == shipmentId)
            );

            return ObjectMapper.Map<List<ShipmentItem>, List<ShipmentItemDto>>(items);
        }

        public async Task<List<ShipmentItemDto>> GetByOrderItemAsync(int orderItemId)
        {
            var queryable = await _shipmentItemRepository.GetQueryableAsync();
            var items = await AsyncExecuter.ToListAsync(
                queryable.Where(si => si.OrderItemId == orderItemId)
            );

            return ObjectMapper.Map<List<ShipmentItem>, List<ShipmentItemDto>>(items);
        }

        public async Task<ShipmentItemDto> CreateAsync(CreateShipmentItemDto input)
        {
            if (!await _shipmentRepository.AnyAsync(s => s.Id == input.ShipmentId))
            {
                throw new UserFriendlyException("Shipment not found!");
            }

            var orderItem = await _orderItemRepository.GetAsync(input.OrderItemId);

            if (input.Quantity <= 0)
            {
                throw new UserFriendlyException("Quantity must be greater than zero!");
            }

            var shippedQuantity = await GetShippedQuantityForOrderItemAsync(input.OrderItemId);
            var availableQuantity = orderItem.Quantity - shippedQuantity;

            if (input.Quantity > availableQuantity)
            {
                throw new UserFriendlyException(
                    $"Cannot ship {input.Quantity} items. Only {availableQuantity} available to ship.");
            }

            var shipmentItem = new ShipmentItem
            {
                ShipmentId = input.ShipmentId,
                OrderItemId = input.OrderItemId,
                Quantity = input.Quantity
            };

            shipmentItem = await _shipmentItemRepository.InsertAsync(shipmentItem);
            return ObjectMapper.Map<ShipmentItem, ShipmentItemDto>(shipmentItem);
        }

        public async Task<ShipmentItemDto> UpdateQuantityAsync(int id, int newQuantity)
        {
            var shipmentItem = await _shipmentItemRepository.GetAsync(id);
            var orderItem = await _orderItemRepository.GetAsync(shipmentItem.OrderItemId);

            if (newQuantity <= 0)
            {
                throw new UserFriendlyException("Quantity must be greater than zero!");
            }

            var shippedQuantity = await GetShippedQuantityForOrderItemAsync(shipmentItem.OrderItemId);
            shippedQuantity -= shipmentItem.Quantity; 
            var availableQuantity = orderItem.Quantity - shippedQuantity;

            if (newQuantity > availableQuantity)
            {
                throw new UserFriendlyException(
                    $"Cannot update to {newQuantity} items. Only {availableQuantity} available to ship.");
            }

            shipmentItem.Quantity = newQuantity;
            shipmentItem = await _shipmentItemRepository.UpdateAsync(shipmentItem);
            return ObjectMapper.Map<ShipmentItem, ShipmentItemDto>(shipmentItem);
        }

        public async Task DeleteAsync(int id)
        {
            await _shipmentItemRepository.DeleteAsync(id);
        }

        public async Task<int> GetShippedQuantityForOrderItemAsync(int orderItemId)
        {
            var queryable = await _shipmentItemRepository.GetQueryableAsync();
            return await AsyncExecuter.SumAsync(
                queryable.Where(si => si.OrderItemId == orderItemId),
                si => si.Quantity
            );
        }
    }

}
