using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Data;
using Todo.Service.Entities;

namespace Todo.Service.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TodoContext context;

        public UserRepository(TodoContext context)
        {
            this.context = context;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await context.Users
                .Include(u => u.UserBoards)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }
    }
}
