using EventManagement.Application.Common;
using EventManagement.Application.DTOs.Category.Command;
using EventManagement.Application.DTOs.Category.Query;
using EventManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EventManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(int pageNumber = 1, int pageSize = 0);
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto dto, string createdBy);
        Task<ServiceResult<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto, string updatedBy);
        Task<ServiceResult<bool>> DeleteAsync(int id, string deletedBy);
    }
}
