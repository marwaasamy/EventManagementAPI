using EventManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EventManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Category?> GetCategoryAsync(
            Expression<Func<Category, bool>>? criteria = null,
            Expression<Func<Category, object>>[]? includes = null,
            string[]? includeStrings = null);

        Task<IEnumerable<Category>> GetAllCategoriesAsync(
            Expression<Func<Category, bool>>? criteria = null,
            Expression<Func<Category, object>>[]? includes = null,
            int pageNumber = 1,
            int pageSize = 0,
            string[]? includeStrings = null);

        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task RemoveCategoryAsync(Category category);
        Task SaveChangesAsync();
    }
}
