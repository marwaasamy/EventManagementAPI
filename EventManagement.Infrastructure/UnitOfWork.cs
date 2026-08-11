using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Data;
using EventManagement.Infrastructure.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Categories = new GenericRepository<Category>(_context);

        }

        public IGenericRepository<Category> Categories { get; }

        public async Task<int> CompleteAsync()
          => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}
