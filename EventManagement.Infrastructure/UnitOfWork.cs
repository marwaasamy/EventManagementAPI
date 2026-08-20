using EventManagement.Application.Interfaces;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Interfaces;
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
            Venues = new GenericRepository<Venue>(_context);
            Events = new GenericRepository<Event>(_context);
            EventRegisters = new GenericRepository<EventRegister>(_context);


        }

        public IGenericRepository<Category> Categories { get; }
        public IGenericRepository<Venue> Venues { get; }

        public IGenericRepository<Event> Events { get; }
        public IGenericRepository<EventRegister> EventRegisters { get; }

        public async Task<int> CompleteAsync()
          => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}
