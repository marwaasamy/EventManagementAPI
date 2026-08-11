using EventManagement.Application.Interfaces;
using EventManagement.Application.Mappings;
using EventManagement.Application.Services;
using EventManagement.Domain.Entities;
using EventManagement.Infrastructure.Data;
using EventManagement.Infrastructure.Helpers;
using EventManagement.Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("cs")));

            // Register generic repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Category service
            services.AddScoped<ICategoryService, CategoryService>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddAutoMapper(cfg => { }, typeof(DomainProfile).Assembly);

            return services;

        }
    }

}
