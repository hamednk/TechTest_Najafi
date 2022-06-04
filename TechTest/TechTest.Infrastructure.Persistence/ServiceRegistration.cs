using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TechTest.Application.Interfaces;
using TechTest.Application.Interfaces.Repositories;
using TechTest.Infrastructure.Persistence.Contexts;
using TechTest.Infrastructure.Persistence.Repositories;
using TechTest.Infrastructure.Persistence.Repository;

namespace TechTest.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            services.AddStackExchangeRedisCache(options => { options.Configuration = configuration["RedisCacheUrl"]; });

            #region Repositories
            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddScoped(typeof(ITitleRepositoryAsync), typeof(TitleRepositoryAsync));
            services.AddScoped(typeof(IProductRepositoryAsync), typeof(ProductRepositoryAsync));
            #endregion
        }
    }
}
