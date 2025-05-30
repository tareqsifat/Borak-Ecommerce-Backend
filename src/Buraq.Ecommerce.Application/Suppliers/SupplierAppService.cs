using Buraq.Ecommerce.Products;
using Buraq.Ecommerce.Suppliers.DTOs;
using Buraq.Ecommerce.Suppliers.Interfaces;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Suppliers
{
    public class SupplierAppService : ApplicationService, ISupplierAppService
    {
        private readonly IRepository<Supplier, int> _supplierRepository;
        private readonly IRepository<Product, int> _productRepository;

        public SupplierAppService(
            IRepository<Supplier, int> supplierRepository,
            IRepository<Product, int> productRepository)
        {
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }

        public async Task<SupplierDto> GetAsync(int id)
        {
            var supplier = await _supplierRepository.GetAsync(id);
            return ObjectMapper.Map<Supplier, SupplierDto>(supplier);
        }

        public async Task<SupplierWithProductsDto> GetWithProductsAsync(int id)
        {
            var supplier = await _supplierRepository.GetAsync(id, includeDetails: true);
            await _supplierRepository.EnsureCollectionLoadedAsync(supplier, s => s.Products);

            return ObjectMapper.Map<Supplier, SupplierWithProductsDto>(supplier);
        }

        public async Task<PagedResultDto<SupplierDto>> GetListAsync(SupplierFilterDto input)
        {
            var queryable = await _supplierRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                queryable = queryable.Where(s => s.Name.Contains(input.Name));
            }

            if (!string.IsNullOrWhiteSpace(input.ContactEmail))
            {
                queryable = queryable.Where(s => s.ContactEmail.Contains(input.ContactEmail));
            }

            if (!string.IsNullOrWhiteSpace(input.PhoneNumber))
            {
                queryable = queryable.Where(s => s.PhoneNumber.Contains(input.PhoneNumber));
            }

            var query = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            query = input.Sorting switch
            {
                "name asc" => query.OrderBy(s => s.Name),
                "name desc" => query.OrderByDescending(s => s.Name),
                "email asc" => query.OrderBy(s => s.ContactEmail),
                "email desc" => query.OrderByDescending(s => s.ContactEmail),
                _ => query.OrderBy(s => s.Name)
            };

            var suppliers = await AsyncExecuter.ToListAsync(query);
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            return new PagedResultDto<SupplierDto>(
                totalCount,
                ObjectMapper.Map<List<Supplier>, List<SupplierDto>>(suppliers)
            );
        }

        public async Task<List<SupplierLookupDto>> GetLookupListAsync()
        {
            var suppliers = await _supplierRepository.GetListAsync();
            return ObjectMapper.Map<List<Supplier>, List<SupplierLookupDto>>(suppliers);
        }

        public async Task<SupplierDto> CreateAsync(CreateUpdateSupplierDto input)
        {
            if (!IsValidEmail(input.ContactEmail))
            {
                throw new UserFriendlyException("Invalid email format!");
            }

            if (!IsValidPhoneNumber(input.PhoneNumber))
            {
                throw new UserFriendlyException("Invalid phone number format!");
            }

            var supplier = new Supplier(
                id: 0, 
                name: input.Name,
                contactEmail: input.ContactEmail,
                phoneNumber: input.PhoneNumber
            );

            supplier = await _supplierRepository.InsertAsync(supplier);
            return ObjectMapper.Map<Supplier, SupplierDto>(supplier);
        }

        public async Task<SupplierDto> UpdateAsync(int id, CreateUpdateSupplierDto input)
        {
            var supplier = await _supplierRepository.GetAsync(id);

            if (supplier.ContactEmail != input.ContactEmail && !IsValidEmail(input.ContactEmail))
            {
                throw new UserFriendlyException("Invalid email format!");
            }

            if (supplier.PhoneNumber != input.PhoneNumber && !IsValidPhoneNumber(input.PhoneNumber))
            {
                throw new UserFriendlyException("Invalid phone number format!");
            }

            supplier.Name = input.Name;
            supplier.ContactEmail = input.ContactEmail;
            supplier.PhoneNumber = input.PhoneNumber;

            supplier = await _supplierRepository.UpdateAsync(supplier);
            return ObjectMapper.Map<Supplier, SupplierDto>(supplier);
        }

        public async Task DeleteAsync(int id)
        {
            var productCount = await GetProductCountAsync(id);
            if (productCount > 0)
            {
                throw new UserFriendlyException("Cannot delete supplier with existing products!");
            }

            await _supplierRepository.DeleteAsync(id);
        }

        public async Task<int> GetProductCountAsync(int supplierId)
        {
            return await _productRepository.CountAsync(p => p.SupplierId == supplierId);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length >= 8;
        }
    }
}
