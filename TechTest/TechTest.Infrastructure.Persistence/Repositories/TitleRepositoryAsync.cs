using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Interfaces.Repositories;
using TechTest.Domain.Entities;
using TechTest.Infrastructure.Persistence.Contexts;
using TechTest.Infrastructure.Persistence.Repository;

namespace TechTest.Infrastructure.Persistence.Repositories
{
    public class TitleRepositoryAsync : GenericRepositoryAsync<Title>, ITitleRepositoryAsync
    {
        private readonly DbSet<Title> _titles;

        public TitleRepositoryAsync(IServiceProvider service) : base(service)
        {
            var dbContext = (ApplicationDbContext)service.GetService(typeof(ApplicationDbContext));
            _titles = dbContext.Set<Title>();
        }

        public Task<bool> IsUniqueToolNameAsync(string toolName)
        {
            return _titles
                .AllAsync(p => p.ToolName != toolName);
        }
    }
}
