using Microsoft.EntityFrameworkCore;
using OpenAI.NET.Web.EntityFrameworkCore.Interfaces;
using OpenAI.NET.Web.EntityFrameworkCore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAI.NET.Web.EntityFrameworkCore.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApiDbContext _apiContext;

        public UserRepository(ApiDbContext apiContext)
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

        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _apiContext.Users.FirstOrDefaultAsync(x => x.Name.Equals(userName));
        }
    }
}