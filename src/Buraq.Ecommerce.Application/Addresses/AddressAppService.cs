using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.Addresses.DTOs;
using Buraq.Ecommerce.Addresses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Addresses
{
    public class AddressAppService : ApplicationService, IAddressAppService
    {
        private readonly IRepository<Address, int> _addressRepository;

        public AddressAppService(
            IRepository<Address, int> 
            addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<AddressDto> GetAsync(int id)
        {
            var address = await _addressRepository.GetAsync(id);
            return ObjectMapper.Map<Address, AddressDto>(address);
        }

        public async Task<List<AddressDto>> GetAllAsync()
        {
            var addresses = await _addressRepository.GetListAsync();
            return ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);
        }

        public async Task<AddressDto> CreateAsync(CreateUpdateAddressDto input)
        {
            var address = new Address(
                id: 0, 
                name: input.Name,
                street: input.Street,
                country: input.Country
            );

            address = await _addressRepository.InsertAsync(address);
            return ObjectMapper.Map<Address, AddressDto>(address);
        }

        public async Task<AddressDto> UpdateAsync(int id, CreateUpdateAddressDto input)
        {
            var address = await _addressRepository.GetAsync(id);

            address.Name = input.Name;
            address.Street = input.Street;
            address.Country = input.Country;

            address = await _addressRepository.UpdateAsync(address);
            return ObjectMapper.Map<Address, AddressDto>(address);
        }

        public async Task DeleteAsync(int id)
        {
            await _addressRepository.DeleteAsync(id);
        }
    }
}
