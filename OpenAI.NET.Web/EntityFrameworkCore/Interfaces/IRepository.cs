using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.EntityFrameworkCore.Interfaces
{
    /// <summary>
    /// Description of database repository.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Async adding item.
        /// </summary>
        public Task AddAsync(T item);
        /// <summary>
        /// Async removing item.
        /// </summary>
        public Task RemoveAsync(T item);
        /// <summary>
        /// Async getting all items.
        /// </summary>
        /// <returns>List of items.</returns>
        public Task<List<T>> GetAllAsync();
    }
}