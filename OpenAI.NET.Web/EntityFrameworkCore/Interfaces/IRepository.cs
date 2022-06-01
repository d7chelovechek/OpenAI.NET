using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.EntityFrameworkCore.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task AddAsync(T item);
        public Task RemoveAsync(T item);
        public Task<List<T>> GetAllAsync();
    }
}