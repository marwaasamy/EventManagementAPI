using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EventManagement.Infrastructure.Repos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext Context;
        protected readonly DbSet<T> DbSet;

        public GenericRepository(AppDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>>? criteria = null,
            Expression<Func<T, object>>[]? includes = null,
            string[]? includeStrings = null)
        {
            IQueryable<T> query = DbSet.Where(e => !e.IsDeleted);

            if (includes is not null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (includeStrings is not null)
                foreach (var includeString in includeStrings)
                    query = query.Include(includeString);

            if (criteria is not null)
                query = query.Where(criteria);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? criteria = null,
            Expression<Func<T, object>>[]? includes = null,
            int pageNumber = 1,
            int pageSize = 0,
            string[]? includeStrings = null)
        {
            IQueryable<T> query = DbSet.Where(e => !e.IsDeleted);

            if (includes is not null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (includeStrings is not null)
                foreach (var includeString in includeStrings)
                    query = query.Include(includeString);

            if (criteria is not null)
                query = query.Where(criteria);

            if (pageSize > 0)
            {
                if (pageNumber < 1) pageNumber = 1;
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

        public void Update(T entity) => DbSet.Update(entity);

        public void Remove(T entity)
        {
            entity.Delete();
            DbSet.Update(entity);
        }
    }
}
