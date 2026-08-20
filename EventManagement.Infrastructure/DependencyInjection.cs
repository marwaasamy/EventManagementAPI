using EventManagement.Application.Interfaces;
using EventManagement.Application.Mappings;
using EventManagement.Application.Services;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Interfaces;
using EventManagement.Infrastructure.Data;
using EventManagement.Infrastructure.Helpers;
using EventManagement.Infrastructure.Repos;
using EventManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            services.AddMemoryCache();

            // Register Category service
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IEventRegisterService, EventRegisterService>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<JWT>(configuration.GetSection("JWT"));

            services.AddIdentity<ApplicationUser, IdentityRole>(
          options =>
          {
              // Configure password requirements
              options.Password.RequireUppercase = true; // Requires at least one uppercase letter
              options.Password.RequireLowercase = true;
              options.Password.RequireDigit = true;
              options.Password.RequireNonAlphanumeric = true;
              options.Password.RequiredLength = 8;
              options.SignIn.RequireConfirmedEmail = true; // Require email confirmation
              options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
          })
         .AddEntityFrameworkStores<AppDbContext>()
         .AddDefaultTokenProviders();

            services.AddAuthentication(options => //how to validate
            {
                options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;//not cookie
                options.DefaultChallengeScheme =//if you aren't valid or have token
                JwtBearerDefaults.AuthenticationScheme; //unauthorized
                options.DefaultScheme =
                JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => //how to verify
            {
                var jwtOptions = configuration.GetSection("JWT").Get<JWT>();

                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters =
                new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.IssuerIP,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.AudienceIP,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                     new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                               jwtOptions.SecretKey))
                };
            });

            services.AddAutoMapper(cfg => { }, typeof(DomainProfile).Assembly);

            return services;

        }
    }

}
