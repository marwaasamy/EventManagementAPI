using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 0)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(pageNumber: pageNumber, pageSize: pageSize);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Category name is required.");
            }

            var category = new Category(request.Name, request.Description ?? string.Empty, GetCurrentUser());
            await _categoryService.AddCategoryAsync(category);
            await _categoryService.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            var category = await _categoryService.GetCategoryAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Category name is required.");
            }

            category.Update(request.Name, request.Description ?? string.Empty, GetCurrentUser());
            await _categoryService.UpdateCategoryAsync(category);
            await _categoryService.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetCategoryAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            category.Delete(GetCurrentUser());
            await _categoryService.RemoveCategoryAsync(category);
            await _categoryService.SaveChangesAsync();

            return NoContent();
        }

        private string GetCurrentUser()
        {
            return User?.Identity?.Name ?? "System";
        }
    }

    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
