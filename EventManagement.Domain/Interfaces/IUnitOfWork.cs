using EventManagement.Domain.Interfaces;
using EventManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Venue> Venues { get; }

        IGenericRepository<Event> Events { get; }
        Task <int> CompleteAsync();
    }
}
