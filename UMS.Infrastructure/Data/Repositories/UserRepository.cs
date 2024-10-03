using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain.Entities;
using UMS.Domain.Interfaces;

namespace UMS.Infrastructure.Data.Repositories
{
    public class UserRepository :Repository<User>, IUserRepository
    {
        public ApplicationDbContext _context => Context as ApplicationDbContext;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            // Check if the username is null or empty
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            }

            // Return the user if found, otherwise return null
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);

            return user;
        }

    }
}
