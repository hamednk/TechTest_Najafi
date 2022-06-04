using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using TechTest.Application.Interfaces;
using TechTest.Infrastructure.Persistence.Contexts;

namespace TechTest.Infrastructure.Persistence.Repository
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;

        public GenericRepositoryAsync(IServiceProvider service)
        {
            _dbContext = (ApplicationDbContext)service.GetService(typeof(ApplicationDbContext));
            _cache = (IDistributedCache)service.GetService(typeof(IDistributedCache));
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    await _dbContext.Set<T>().AddRangeAsync(entity);
                    await _dbContext.SaveChangesAsync();
                }
                finally
                {
                    if (_dbContext != null)
                        _dbContext.Dispose();
                }

                scope.Complete();
            }
        }

        public async Task AddBulkAsync(List<T> entity)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    await _dbContext.BulkInsertAsync(entity, option => option.BatchSize = 500000);
                }
                finally
                {
                    if (_dbContext != null)
                        _dbContext.Dispose();
                }

                scope.Complete();
            }
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext
                 .Set<T>()
                 .ToListAsync();
        }

        public async Task SetToRedisAsync(IEnumerable<T> entity, string cacheKey)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                   .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                   .SetSlidingExpiration(TimeSpan.FromMinutes(3));

            string cachedDataString = JsonSerializer.Serialize(entity.ToArray());
            var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

            await _cache.SetAsync(cacheKey, dataToCache, options);
        }

        public virtual async Task<List<T>> GetFromRedisAsync(string cacheKey)
        {
            byte[] cachedData = await _cache.GetAsync(cacheKey);

            var cachedDataString = Encoding.UTF8.GetString(cachedData);
            var data = JsonSerializer.Deserialize<List<T>>(cachedDataString);

            return data;
        }

        public async Task RemoveFromRedisAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }



    }
}
