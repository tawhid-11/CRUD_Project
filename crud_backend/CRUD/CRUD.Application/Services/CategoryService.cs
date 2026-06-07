using CRUD.Application.Category_Dto;
using CRUD.Application.Service_Interfaces;
using CRUD.Application.UOW_Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _uow.categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var c = await _uow.categoryRepository.GetByIdAsync(id);
            if (c == null)
                return null;
            return new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            };
        }
        public async Task<int> CreateCategoryAsync(CreateCategoryDto cco)
        {
            if (string.IsNullOrWhiteSpace(cco.Name))
                throw new ArgumentException("Category name cannot be empty");
            var category = new Core.Entities.Category
            {
                Name = cco.Name
            };
            return await _uow.categoryRepository.CreateAsync(category);
        }
        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDto uco)
        {
            if (uco == null)
                throw new ArgumentNullException(nameof(uco));

            if (string.IsNullOrWhiteSpace(uco.Name))
                throw new ArgumentException("Category name cannot be empty");

            var existingCategory = await _uow.categoryRepository.GetByIdAsync(uco.Id);

            if (existingCategory == null)
                throw new KeyNotFoundException("Category not found");

            existingCategory.Name = uco.Name;

            return await _uow.categoryRepository.UpdateAsync(existingCategory);
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var existingCategory = await _uow.categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
                return false;
            return await _uow.categoryRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<CategoryDto>> SearchAsync(string text)
        {
            var data = await _uow.categoryRepository.SearchAsync(text);

            return data.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

    }
}
