using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.CartItems.DTOs;
using Buraq.Ecommerce.CartItems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.CartItems
{
    public class CartItemAppService : ApplicationService, ICartItemAppService
    {
        private readonly IRepository<CartItem, int> _cartItemRepository;

        public CartItemAppService(
            IRepository<CartItem, int> cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<CartItemDto> GetAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetAsync(id);
            return ObjectMapper.Map<CartItem, CartItemDto>(cartItem);
        }

        public async Task<PagedResultDto<CartItemDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _cartItemRepository.GetQueryableAsync();
            
            var query = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            if (!string.IsNullOrEmpty(input.Sorting))
            {
                // Implement sorting logic here if needed
            }

            var items = await AsyncExecuter.ToListAsync(query);
            var totalCount = await _cartItemRepository.GetCountAsync();

            return new PagedResultDto<CartItemDto>(
                totalCount,
                ObjectMapper.Map<List<CartItem>, List<CartItemDto>>(items)
            );
        }

        public async Task<List<CartItemDto>> GetItemsByCartIdAsync(int cartId)
        {
            var queryable = await _cartItemRepository.GetQueryableAsync();
            
            var items = await AsyncExecuter
                .ToListAsync(queryable.Where(ci => ci.CartId == cartId));

            return ObjectMapper.Map<List<CartItem>, List<CartItemDto>>(items);
        }

        public async Task<CartItemDto> CreateAsync(CreateUpdateCartItemDto input)
        {
            var cartItem = new CartItem(
                id: 0, 
                cartId: input.CartId,
                productId: input.ProductId,
                quantity: input.Quantity
            );

            cartItem = await _cartItemRepository.InsertAsync(cartItem);
            return ObjectMapper.Map<CartItem, CartItemDto>(cartItem);
        }

        public async Task<CartItemDto> UpdateAsync(int id, CreateUpdateCartItemDto input)
        {
            var cartItem = await _cartItemRepository.GetAsync(id);

            cartItem.CartId = input.CartId;
            cartItem.ProductId = input.ProductId;
            cartItem.Quantity = input.Quantity;

            cartItem = await _cartItemRepository.UpdateAsync(cartItem);
            return ObjectMapper.Map<CartItem, CartItemDto>(cartItem);
        }

        public async Task UpdateQuantityAsync(int id, int newQuantity)
        {
            var cartItem = await _cartItemRepository.GetAsync(id);
            cartItem.Quantity = newQuantity;
            await _cartItemRepository.UpdateAsync(cartItem);
        }

        public async Task DeleteAsync(int id)
        {
            await _cartItemRepository.DeleteAsync(id);
        }

        public async Task ClearCartAsync(int cartId)
        {
            var queryable = await _cartItemRepository.GetQueryableAsync();
            
            var itemsToDelete = await AsyncExecuter
                .ToListAsync(queryable.Where(ci => ci.CartId == cartId));

            await _cartItemRepository.DeleteManyAsync(itemsToDelete);
        }
    }
}
