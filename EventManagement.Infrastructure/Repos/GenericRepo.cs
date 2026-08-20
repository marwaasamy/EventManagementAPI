using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Interfaces;
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

        public Task<T> UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);
            return Task.FromResult(entity);
        }

        public Task SoftDeleteAsync(T entity, string deletedUser = "")
        {
            entity.Delete(deletedUser);
            Context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task HardDeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}
