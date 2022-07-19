using Microsoft.EntityFrameworkCore;
using OpenAI.NET.Web.EntityFrameworkCore.Entities;
using OpenAI.NET.Web.EntityFrameworkCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// User repository.
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _apiContext;

        /// <summary>
        /// A constructor that initializes all fields.
        /// </summary>
        public UserRepository(AppDbContext apiContext)
        {
            _apiContext = apiContext;
        }

        public async Task AddAsync(User user)
        {
            _apiContext.Add(user);
            await _apiContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(User user)
        {
            _apiContext.Remove(user);
            await _apiContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _apiContext.Users.ToListAsync();
        }

        /// <summary>
        /// Async getting user by User name.
        /// </summary>
        /// <returns>User</returns>
        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _apiContext.Users
                .FirstOrDefaultAsync(x => x.Name.Equals(userName));
        }
    }
}