using EventManagement.Application.DTOs.Category.Command;
using EventManagement.Application.DTOs.Category.Query;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 0) =>
            Ok(await _categoryService.GetAllAsync(pageNumber, pageSize));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto dto)
        {
            var createdBy = User.Identity?.Name ?? "system"; // swap once JWT auth is in
            var result = await _categoryService.CreateAsync(dto, createdBy);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Update(int id, UpdateCategoryDto dto)
        {
            var updatedBy = User.Identity?.Name ?? "system";
            var result = await _categoryService.UpdateAsync(id, dto, updatedBy);
            return result.Success ? Ok(result.Data) : BadRequest(new { message = result.Message });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedBy = User.Identity?.Name ?? "system";
            var result = await _categoryService.DeleteAsync(id, deletedBy);
            return result.Success ? NoContent() : BadRequest(new { message = result.Message });
        }
    }

}
