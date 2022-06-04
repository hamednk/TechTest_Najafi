using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechTest.Domain.Entities;

namespace TechTest.Application.Interfaces.Repositories
{
    public interface ITitleRepositoryAsync : IGenericRepositoryAsync<Title>
    {
        Task<bool> IsUniqueToolNameAsync(string toolName);
    }
}
