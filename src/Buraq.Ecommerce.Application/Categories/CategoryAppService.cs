using AutoMapper.Internal.Mappers;
using Buraq.Ecommerce.Categories.DTOs;
using Buraq.Ecommerce.Categories.Interfaces;
using Buraq.Ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Buraq.Ecommerce.Categories
{
    public class CategoryAppService : ApplicationService, ICategoryAppService
    {
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly IRepository<Product, int> _productRepository;

        public CategoryAppService(
            IRepository<Category, int> categoryRepository,
            IRepository<Product, int> productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<CategoryDto> GetAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        public async Task<CategoryWithProductsDto> GetWithProductsAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id, includeDetails: true);
            await _categoryRepository.EnsureCollectionLoadedAsync(category, c => c.Products);

            var dto = ObjectMapper.Map<Category, CategoryWithProductsDto>(category);
            return dto;
        }

        public async Task<PagedResultDto<CategoryDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _categoryRepository.GetQueryableAsync();
            var query = queryable.Skip(input.SkipCount)
                                .Take(input.MaxResultCount);

            if (!string.IsNullOrEmpty(input.Sorting))
            {
                query = input.Sorting switch
                {
                    "name asc" => query.OrderBy(c => c.Name),
                    "name desc" => query.OrderByDescending(c => c.Name),
                    _ => query.OrderBy(c => c.CreationTime)
                };
            }

            var categories = await AsyncExecuter.ToListAsync(query);
            var totalCount = await _categoryRepository.GetCountAsync();

            return new PagedResultDto<CategoryDto>(
                totalCount,
                ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories)
            );
        }

        public async Task<List<CategoryLookupDto>> GetLookupListAsync()
        {
            var categories = await _categoryRepository.GetListAsync();
            return ObjectMapper.Map<List<Category>, List<CategoryLookupDto>>(categories);
        }

        public async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
        {
            var category = new Category(
                id: 0, 
                name: input.Name,
                description: input.Description,
                image: input.Image
            );

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new UserFriendlyException("Category name cannot be empty!");
            }

            category = await _categoryRepository.InsertAsync(category);
            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateAsync(int id, CreateUpdateCategoryDto input)
        {
            var category = await _categoryRepository.GetAsync(id);

            category.Name = input.Name;
            category.Description = input.Description;
            category.Image = input.Image;

            category = await _categoryRepository.UpdateAsync(category);
            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        public async Task DeleteAsync(int id)
        {
            var productCount = await GetProductCountAsync(id);
            if (productCount > 0)
            {
                throw new UserFriendlyException("Cannot delete category with existing products!");
            }

            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<int> GetProductCountAsync(int categoryId)
        {
            var queryable = await _productRepository.GetQueryableAsync();
            return await AsyncExecuter.CountAsync(
                queryable.Where(p => p.CategoryId == categoryId)
            );
        }
    }
}
