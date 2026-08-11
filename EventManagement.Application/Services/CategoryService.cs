using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EventManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Category?> GetCategoryAsync(
            Expression<Func<Category, bool>>? criteria = null,
            Expression<Func<Category, object>>[]? includes = null,
            string[]? includeStrings = null)
        {
            return await _repository.GetAsync(criteria, includes, includeStrings);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(
            Expression<Func<Category, bool>>? criteria = null,
            Expression<Func<Category, object>>[]? includes = null,
            int pageNumber = 1,
            int pageSize = 0,
            string[]? includeStrings = null)
        {
            return await _repository.GetAllAsync(criteria, includes, pageNumber, pageSize, includeStrings);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _repository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _repository.Update(category);
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            _repository.Remove(category);
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
