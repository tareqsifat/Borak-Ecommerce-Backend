using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.Addresses;
using Buraq.Ecommerce.Carts;
using Buraq.Ecommerce.Carts.DTOs;
using Buraq.Ecommerce.Customers.DTOs;
using Buraq.Ecommerce.Customers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Customers
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<Address, int> _addressRepository;
        private readonly IRepository<Cart, int> _cartRepository;

        public CustomerAppService(
            IRepository<Customer, int> customerRepository,
            IRepository<Address, int> addressRepository,
            IRepository<Cart, int> cartRepository)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _cartRepository = cartRepository;
        }

        public async Task<CustomerDto> GetAsync(int id)
        {
            var customer = await _customerRepository.GetAsync(id);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        public async Task<CustomerWithDetailsDto> GetWithDetailsAsync(int id)
        {
            var customer = await _customerRepository.GetAsync(id, includeDetails: true);
            await _customerRepository.EnsurePropertyLoadedAsync(customer, c => c.Address);

            var dto = ObjectMapper.Map<Customer, CustomerWithDetailsDto>(customer);

            var carts = await _cartRepository.GetListAsync(c => c.CustomerId == id);
            dto.Carts = ObjectMapper.Map<List<Cart>, List<CartDto>>(carts);

            return dto;
        }

        public async Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerListDto input)
        {
            var queryable = await _customerRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(c =>
                    c.Name.Contains(input.Filter) ||
                    c.Email.Contains(input.Filter) ||
                    c.Phone.Contains(input.Filter));
            }

            var query = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            query = input.Sorting switch
            {
                "name asc" => query.OrderBy(c => c.Name),
                "name desc" => query.OrderByDescending(c => c.Name),
                "email asc" => query.OrderBy(c => c.Email),
                "email desc" => query.OrderByDescending(c => c.Email),
                _ => query.OrderBy(c => c.CreationTime)
            };

            var customers = await AsyncExecuter.ToListAsync(query);
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            return new PagedResultDto<CustomerDto>(
                totalCount,
                ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers)
            );
        }

        public async Task<List<CustomerLookupDto>> GetLookupListAsync()
        {
            var customers = await _customerRepository.GetListAsync();
            return ObjectMapper.Map<List<Customer>, List<CustomerLookupDto>>(customers);
        }

        public async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
        {
            if (await _customerRepository.AnyAsync(c => c.Email == input.Email))
            {
                throw new UserFriendlyException("Email address is already in use!");
            }

            var customer = new Customer(
                id: 0,
                name: input.Name,
                email: input.Email,
                phone: input.Phone,
                addressId: input.AddressId
            );

            customer = await _customerRepository.InsertAsync(customer);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        public async Task<CustomerDto> UpdateAsync(int id, CreateUpdateCustomerDto input)
        {
            var customer = await _customerRepository.GetAsync(id);

            if (customer.Email != input.Email &&
                await _customerRepository.AnyAsync(c => c.Email == input.Email))
            {
                throw new UserFriendlyException("Email address is already in use!");
            }

            customer.Name = input.Name;
            customer.Email = input.Email;
            customer.Phone = input.Phone;
            customer.AddressId = input.AddressId;

            customer = await _customerRepository.UpdateAsync(customer);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        public async Task DeleteAsync(int id)
        {
            var cartCount = await GetCartCountAsync(id);
            if (cartCount > 0)
            {
                throw new UserFriendlyException("Cannot delete customer with existing carts!");
            }

            await _customerRepository.DeleteAsync(id);
        }

        public async Task<int> GetCartCountAsync(int customerId)
        {
            return await _cartRepository.CountAsync(c => c.CustomerId == customerId);
        }
    }

}
