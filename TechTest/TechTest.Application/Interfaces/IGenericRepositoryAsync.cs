using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Application.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task AddRangeAsync(IEnumerable<T> entity);
        Task AddBulkAsync(List<T> entity);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task SetToRedisAsync(IEnumerable<T> entity, string cacheKey);
        Task<List<T>> GetFromRedisAsync(string cacheKey);
        Task RemoveFromRedisAsync(string cacheKey);

    }
}
