using CRUD.Application.Category_Dto;


namespace CRUD.Application.Service_Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        public Task<CategoryDto?> GetCategoryByIdAsync(int id);
        public Task<int> CreateCategoryAsync(CreateCategoryDto categoryDto);
        public Task<bool> UpdateCategoryAsync(UpdateCategoryDto categoryDto);
        public Task<bool> DeleteCategoryAsync(int id);
        public Task<IEnumerable<CategoryDto>> SearchAsync(string text);
    }
}
