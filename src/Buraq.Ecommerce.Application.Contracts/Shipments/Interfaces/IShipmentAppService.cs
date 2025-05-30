using Buraq.Ecommerce.Enums;
using Buraq.Ecommerce.Shipments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.Shipments.Interfaces
{
    public interface IShipmentAppService : IApplicationService
    {
        Task<ShipmentDto> GetAsync(int id);
        Task<ShipmentWithItemsDto> GetWithItemsAsync(int id);
        Task<PagedResultDto<ShipmentDto>> GetListAsync(ShipmentFilterDto input);
        Task<List<ShipmentDto>> GetByOrderAsync(int orderId);
        Task<ShipmentDto> CreateAsync(CreateShipmentDto input);
        Task<ShipmentDto> UpdateAsync(int id, UpdateShipmentDto input);
        Task UpdateStatusAsync(int id, ShipmentStatus status);
        Task AddTrackingInfoAsync(int id, string carrier, string trackingNumber);
        Task DeleteAsync(int id);
        Task<int> GetItemCountAsync(int shipmentId);
        Task<ShipmentSummaryDto> GetShipmentSummaryAsync(int id);
    }
}
