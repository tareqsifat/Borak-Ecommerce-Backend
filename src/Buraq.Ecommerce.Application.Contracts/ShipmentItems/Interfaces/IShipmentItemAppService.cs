using Buraq.Ecommerce.ShipmentItems.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce.ShipmentItems.Interfaces
{
    public interface IShipmentItemAppService : IApplicationService
    {
        Task<ShipmentItemDto> GetAsync(int id);
        Task<List<ShipmentItemDto>> GetByShipmentAsync(int shipmentId);
        Task<List<ShipmentItemDto>> GetByOrderItemAsync(int orderItemId);
        Task<ShipmentItemDto> CreateAsync(CreateShipmentItemDto input);
        Task<ShipmentItemDto> UpdateQuantityAsync(int id, int newQuantity);
        Task DeleteAsync(int id);
        Task<int> GetShippedQuantityForOrderItemAsync(int orderItemId);
    }
}
