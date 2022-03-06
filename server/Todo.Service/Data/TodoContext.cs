using Microsoft.EntityFrameworkCore;
using Todo.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Todo.Service.Data
{
    public class TodoContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public TodoContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TodoItem> Todos { get; set; }

        public DbSet<Board> Boards { get; set; }
    }
}
