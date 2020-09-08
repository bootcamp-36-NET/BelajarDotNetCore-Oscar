using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<int> Update(T entity);
        Task<int> Create(T entity);
        Task<int> Delete(int id);
    }
}
