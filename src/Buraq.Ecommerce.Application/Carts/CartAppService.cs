using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.CartItems;
using Buraq.Ecommerce.Carts.DTOs;
using Buraq.Ecommerce.Carts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Carts
{
    public class CartAppService : ApplicationService, ICartAppService
    {
        private readonly IRepository<Cart, int> _cartRepository;
        private readonly IRepository<CartItem, int> _cartItemRepository;

        public CartAppService(
            IRepository<Cart, int> cartRepository,
            IRepository<CartItem, int> cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<CartDto> GetAsync(int id)
        {
            var cart = await _cartRepository.GetAsync(id);
            return ObjectMapper.Map<Cart, CartDto>(cart);
        }

        public async Task<CartWithItemsDto> GetWithItemsAsync(int id)
        {
            var cart = await _cartRepository.GetAsync(id, includeDetails: true);
            return ObjectMapper.Map<Cart, CartWithItemsDto>(cart);
        }

        public async Task<CartDto> GetByCustomerAsync(int customerId)
        {
            var queryable = await _cartRepository.GetQueryableAsync();
            
            var cart = await AsyncExecuter
                .FirstOrDefaultAsync(queryable.Where(c => c.CustomerId == customerId));

            return ObjectMapper.Map<Cart, CartDto>(cart);
        }

        public async Task<CartWithItemsDto> GetByCustomerWithItemsAsync(int customerId)
        {
            var queryable = await _cartRepository.GetQueryableAsync();

            var cart = await AsyncExecuter
                .FirstOrDefaultAsync(queryable.Where(c => c.CustomerId == customerId));

            if (cart != null)
            {
                await _cartRepository.EnsureCollectionLoadedAsync(cart, c => c.CartItems);
            }

            return ObjectMapper.Map<Cart, CartWithItemsDto>(cart);
        }

        public async Task<PagedResultDto<CartDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _cartRepository.GetQueryableAsync();
            var query = queryable.Skip(input.SkipCount)
                                .Take(input.MaxResultCount);

            if (!string.IsNullOrEmpty(input.Sorting))
            {
                // Implement sorting logic based on input.Sorting
            }

            var carts = await AsyncExecuter.ToListAsync(query);
            var totalCount = await _cartRepository.GetCountAsync();

            return new PagedResultDto<CartDto>(
                totalCount,
                ObjectMapper.Map<List<Cart>, List<CartDto>>(carts)
            );
        }

        public async Task<CartDto> CreateAsync(CreateOrUpdateCartDto input)
        {
            var cart = new Cart(
                id: 0, 
                customerId: input.CustomerId
            );

            cart = await _cartRepository.InsertAsync(cart);
            return ObjectMapper.Map<Cart, CartDto>(cart);
        }

        public async Task<CartDto> UpdateAsync(int id, CreateOrUpdateCartDto input)
        {
            var cart = await _cartRepository.GetAsync(id);

            cart.CustomerId = input.CustomerId;

            cart = await _cartRepository.UpdateAsync(cart);
            return ObjectMapper.Map<Cart, CartDto>(cart);
        }

        public async Task DeleteAsync(int id)
        {
            await _cartItemRepository.DeleteAsync(ci => ci.CartId == id);
            await _cartRepository.DeleteAsync(id);
        }

        public async Task<int> GetItemCountAsync(int cartId)
        {
            var queryable = await _cartItemRepository.GetQueryableAsync();
            return await AsyncExecuter.CountAsync(
                queryable.Where(ci => ci.CartId == cartId)
            );
        }

        public async Task<decimal> CalculateTotalAsync(int cartId)
        {
            var queryable = await _cartItemRepository.GetQueryableAsync();
            var items = await AsyncExecuter.ToListAsync(queryable.Where(ci => ci.CartId == cartId));

            decimal total = 0;
            foreach (var item in items)
            {
                total += item.Quantity * item.Product.Price;
            }

            return total;
        }
    }

}
