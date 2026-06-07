using CRUD.Application.Category_Dto;
using CRUD.Application.Service_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null)
                return BadRequest("DTO cannot be null");
            var id = await _categoryService.CreateCategoryAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetById), new { id = id }, createCategoryDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (updateCategoryDto == null)
                return BadRequest("DTO cannot be null");
            if (id != updateCategoryDto.Id)
                return BadRequest("ID Mismatch");
            var result = await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            if (!result)
                return NotFound();
            return Ok(new { message = "Category updated successfully" });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
                return NotFound();
            return Ok(new { message = "Category deleted successfully" });
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string text)
        {
            var result = await _categoryService.SearchAsync(text);
            return Ok(result);
        }
    }
}
