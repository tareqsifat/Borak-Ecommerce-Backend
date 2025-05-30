using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.Enums;
using Buraq.Ecommerce.Orders;
using Buraq.Ecommerce.ShipmentItems;
using Buraq.Ecommerce.Shipments.DTOs;
using Buraq.Ecommerce.Shipments.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Shipments
{
    public class ShipmentAppService : ApplicationService, IShipmentAppService
    {
        private readonly IRepository<Shipment, int> _shipmentRepository;
        private readonly IRepository<Order, int> _orderRepository;
        private readonly IRepository<ShipmentItem, int> _shipmentItemRepository;

        public ShipmentAppService(
            IRepository<Shipment, int> shipmentRepository,
            IRepository<Order, int> orderRepository,
            IRepository<ShipmentItem, int> shipmentItemRepository)
        {
            _shipmentRepository = shipmentRepository;
            _orderRepository = orderRepository;
            _shipmentItemRepository = shipmentItemRepository;
        }

        public async Task<ShipmentDto> GetAsync(int id)
        {
            var shipment = await _shipmentRepository.GetAsync(id);
            return ObjectMapper.Map<Shipment, ShipmentDto>(shipment);
        }

        public async Task<ShipmentWithItemsDto> GetWithItemsAsync(int id)
        {
            var shipment = await _shipmentRepository.GetAsync(id, includeDetails: true);
            await _shipmentRepository.EnsureCollectionLoadedAsync(shipment, s => s.Items);

            var dto = ObjectMapper.Map<Shipment, ShipmentWithItemsDto>(shipment);
            return dto;
        }

        public async Task<PagedResultDto<ShipmentDto>> GetListAsync(ShipmentFilterDto input)
        {
            var queryable = await _shipmentRepository.GetQueryableAsync();

            if (input.OrderId.HasValue)
            {
                queryable = queryable.Where(s => s.OrderId == input.OrderId.Value);
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(s => s.Status == input.Status.Value);
            }

            if (input.StartDate.HasValue)
            {
                queryable = queryable.Where(s => s.ShipmentDate >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                queryable = queryable.Where(s => s.ShipmentDate <= input.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(input.TrackingNumber))
            {
                queryable = queryable.Where(s => s.TrackingNumber.Contains(input.TrackingNumber));
            }

            var query = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            query = input.Sorting switch
            {
                "date asc" => query.OrderBy(s => s.ShipmentDate),
                "date desc" => query.OrderByDescending(s => s.ShipmentDate),
                _ => query.OrderByDescending(s => s.ShipmentDate)
            };

            var shipments = await AsyncExecuter.ToListAsync(query);
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            return new PagedResultDto<ShipmentDto>(
                totalCount,
                ObjectMapper.Map<List<Shipment>, List<ShipmentDto>>(shipments)
            );
        }

        public async Task<List<ShipmentDto>> GetByOrderAsync(int orderId)
        {
            var queryable = await _shipmentRepository.GetQueryableAsync();
            var shipments = await AsyncExecuter.ToListAsync(
                queryable.Where(s => s.OrderId == orderId)
                    .OrderByDescending(s => s.ShipmentDate)
            );

            return ObjectMapper.Map<List<Shipment>, List<ShipmentDto>>(shipments);
        }

        public async Task<ShipmentDto> CreateAsync(CreateShipmentDto input)
        {
            if (!await _orderRepository.AnyAsync(o => o.Id == input.OrderId))
            {
                throw new UserFriendlyException("Order not found!");
            }

            var shipment = new Shipment(
                id: 0, 
                orderId: input.OrderId,
                shipmentDate: DateTime.Now,
                carrier: input.Carrier,
                trackingNumber: input.TrackingNumber,
                status: ShipmentStatus.Pending
            );

            shipment = await _shipmentRepository.InsertAsync(shipment);
            return ObjectMapper.Map<Shipment, ShipmentDto>(shipment);
        }

        public async Task<ShipmentDto> UpdateAsync(int id, UpdateShipmentDto input)
        {
            var shipment = await _shipmentRepository.GetAsync(id);

            if (input.Carrier != null)
            {
                shipment.Carrier = input.Carrier;
            }

            if (input.TrackingNumber != null)
            {
                shipment.TrackingNumber = input.TrackingNumber;
            }

            shipment = await _shipmentRepository.UpdateAsync(shipment);
            return ObjectMapper.Map<Shipment, ShipmentDto>(shipment);
        }

        public async Task UpdateStatusAsync(int id, ShipmentStatus status)
        {
            var shipment = await _shipmentRepository.GetAsync(id);

            if (shipment.Status == ShipmentStatus.Delivered && status != ShipmentStatus.Delivered)
            {
                throw new UserFriendlyException("Cannot change status from Delivered!");
            }

            shipment.Status = status;

            if (status == ShipmentStatus.Shipped && shipment.ShipmentDate == default)
            {
                shipment.ShipmentDate = DateTime.Now;
            }

            await _shipmentRepository.UpdateAsync(shipment);
        }

        public async Task AddTrackingInfoAsync(int id, string carrier, string trackingNumber)
        {
            var shipment = await _shipmentRepository.GetAsync(id);

            shipment.Carrier = carrier;
            shipment.TrackingNumber = trackingNumber;

            if (shipment.Status == ShipmentStatus.Pending)
            {
                shipment.Status = ShipmentStatus.Processing;
            }

            await _shipmentRepository.UpdateAsync(shipment);
        }

        public async Task DeleteAsync(int id)
        {
            var shipment = await _shipmentRepository.GetAsync(id);

            if (shipment.Status == ShipmentStatus.Shipped ||
                shipment.Status == ShipmentStatus.Delivered)
            {
                throw new UserFriendlyException("Cannot delete shipped or delivered shipments!");
            }

            await _shipmentItemRepository.DeleteAsync(si => si.ShipmentId == id);

            await _shipmentRepository.DeleteAsync(id);
        }

        public async Task<int> GetItemCountAsync(int shipmentId)
        {
            return await _shipmentItemRepository.CountAsync(si => si.ShipmentId == shipmentId);
        }

        public async Task<ShipmentSummaryDto> GetShipmentSummaryAsync(int id)
        {
            var shipment = await GetWithItemsAsync(id);
            var order = await _orderRepository.GetAsync(shipment.OrderId);

            return new ShipmentSummaryDto
            {
                ShipmentId = id,
                OrderId = shipment.OrderId,
                OrderNumber = order.Id.ToString(), 
                Status = shipment.Status,
                ShipmentDate = shipment.ShipmentDate,
                Carrier = shipment.Carrier,
                TrackingNumber = shipment.TrackingNumber,
                ItemCount = shipment.Items.Count
            };
        }
    }
}
