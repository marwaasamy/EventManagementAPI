using EventManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EventManagement.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(
            Expression<Func<T, bool>>? criteria = null,
            Expression<Func<T, object>>[]? includes = null,
            string[]? includeStrings = null);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? criteria = null,
            Expression<Func<T, object>>[]? includes = null,
            int pageNumber = 1,
            int pageSize = 0,
            string[]? includeStrings = null);

        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
