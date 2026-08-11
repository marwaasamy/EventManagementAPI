using AutoMapper;
using EventManagement.Application.Common;
using EventManagement.Application.DTOs.Category.Command;
using EventManagement.Application.DTOs.Category.Query;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(int pageNumber = 1, int pageSize = 0)
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(
                includeStrings: new[] { "Events" },
                pageNumber: pageNumber,
                pageSize: pageSize);

            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(
                criteria: c => c.Id == id,
                includeStrings: new[] { "Events" });

            return category is null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto dto, string createdBy)
        {
            var existing = await _unitOfWork.Categories.GetAsync(criteria: c => c.Name == dto.Name);
            if (existing is not null)
                return ServiceResult<CategoryDto>.Fail("A category with this name already exists.");

            var category = new Category(dto.Name, dto.Description, createdBy);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<CategoryDto>.Ok(_mapper.Map<CategoryDto>(category));
        }

        public async Task<ServiceResult<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto, string updatedBy)
        {
            var category = await _unitOfWork.Categories.GetAsync(criteria: c => c.Id == id);
            if (category is null)
                return ServiceResult<CategoryDto>.Fail("Category not found.");

            category.Update(dto.Name, dto.Description, updatedBy);
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<CategoryDto>.Ok(_mapper.Map<CategoryDto>(category));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id, string deletedBy)
        {
            var category = await _unitOfWork.Categories.GetAsync(criteria: c => c.Id == id);
            if (category is null)
                return ServiceResult<bool>.Fail("Category not found.");

            category.Delete(deletedBy); // soft delete via domain method
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<bool>.Ok(true);
        }
    }
}
