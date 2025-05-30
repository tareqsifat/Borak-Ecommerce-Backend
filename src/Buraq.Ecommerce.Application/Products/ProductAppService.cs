using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.Categories;
using Buraq.Ecommerce.Products.DTOs;
using Buraq.Ecommerce.Products.Interfaces;
using Buraq.Ecommerce.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Products
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly IRepository<Supplier, int> _supplierRepository;

        public ProductAppService(
            IRepository<Product, int> productRepository,
            IRepository<Category, int> categoryRepository,
            IRepository<Supplier, int> supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<ProductWithDetailsDto> GetWithDetailsAsync(int id)
        {
            var product = await _productRepository.GetAsync(id, includeDetails: true);
            await _productRepository.EnsurePropertyLoadedAsync(product, p => p.Category);
            await _productRepository.EnsurePropertyLoadedAsync(product, p => p.Supplier);

            return ObjectMapper.Map<Product, ProductWithDetailsDto>(product);
        }

        public async Task<PagedResultDto<ProductDto>> GetListAsync(ProductFilterDto input)
        {
            var queryable = await _productRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                queryable = queryable.Where(p => p.Name.Contains(input.Name));
            }

            if (input.CategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.CategoryId == input.CategoryId.Value);
            }

            if (input.SupplierId.HasValue)
            {
                queryable = queryable.Where(p => p.SupplierId == input.SupplierId.Value);
            }

            if (input.MinPrice.HasValue)
            {
                queryable = queryable.Where(p => p.Price >= input.MinPrice.Value);
            }

            if (input.MaxPrice.HasValue)
            {
                queryable = queryable.Where(p => p.Price <= input.MaxPrice.Value);
            }

            if (input.IsActive.HasValue)
            {
                queryable = queryable.Where(p => p.IsActive == input.IsActive.Value);
            }

            var query = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            query = input.Sorting switch
            {
                "name asc" => query.OrderBy(p => p.Name),
                "name desc" => query.OrderByDescending(p => p.Name),
                "price asc" => query.OrderBy(p => p.Price),
                "price desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            var products = await AsyncExecuter.ToListAsync(query);
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            return new PagedResultDto<ProductDto>(
                totalCount,
                ObjectMapper.Map<List<Product>, List<ProductDto>>(products)
            );
        }

        public async Task<List<ProductLookupDto>> GetActiveProductsAsync()
        {
            var products = await _productRepository.GetListAsync(p => p.IsActive);
            return ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(products);
        }

        public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            if (!await _categoryRepository.AnyAsync(c => c.Id == input.CategoryId))
            {
                throw new UserFriendlyException("Category not found!");
            }

            if (!await _supplierRepository.AnyAsync(s => s.Id == input.SupplierId))
            {
                throw new UserFriendlyException("Supplier not found!");
            }

            if (input.Price <= 0)
            {
                throw new UserFriendlyException("Price must be greater than zero!");
            }

            var product = new Product(
                id: 0, 
                name: input.Name,
                description: input.Description,
                price: input.Price,
                stockQuantity: input.StockQuantity,
                categoryId: input.CategoryId,
                supplierId: input.SupplierId,
                isActive: input.IsActive
            );

            product = await _productRepository.InsertAsync(product);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<ProductDto> UpdateAsync(int id, CreateUpdateProductDto input)
        {
            var product = await _productRepository.GetAsync(id);

            if (product.CategoryId != input.CategoryId &&
                !await _categoryRepository.AnyAsync(c => c.Id == input.CategoryId))
            {
                throw new UserFriendlyException("Category not found!");
            }

            if (product.SupplierId != input.SupplierId &&
                !await _supplierRepository.AnyAsync(s => s.Id == input.SupplierId))
            {
                throw new UserFriendlyException("Supplier not found!");
            }

            product.Name = input.Name;
            product.Description = input.Description;
            product.Price = input.Price;
            product.StockQuantity = input.StockQuantity;
            product.CategoryId = input.CategoryId;
            product.SupplierId = input.SupplierId;
            product.IsActive = input.IsActive;

            product = await _productRepository.UpdateAsync(product);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task ToggleStatusAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            product.IsActive = !product.IsActive;
            await _productRepository.UpdateAsync(product);
        }

        public async Task UpdateStockAsync(int id, int quantityChange)
        {
            var product = await _productRepository.GetAsync(id);
            product.StockQuantity += quantityChange;

            if (product.StockQuantity < 0)
            {
                throw new UserFriendlyException("Insufficient stock quantity!");
            }

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);

            if (product.StockQuantity > 0)
            {
                throw new UserFriendlyException("Cannot delete product with existing inventory!");
            }

            await _productRepository.DeleteAsync(id);
        }
    }
}
