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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserBoard>()
                .HasKey(ub => new { ub.UserId, ub.BoardId });

            modelBuilder.Entity<UserBoard>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserBoards)
                .HasForeignKey(ub => ub.UserId);

            modelBuilder.Entity<UserBoard>()
              .HasOne(ub => ub.Board)
              .WithMany(u => u.UserBoards)
              .HasForeignKey(ub => ub.BoardId);

            modelBuilder.Entity<BoardPermission>()
                .Property(x => x.Permission).HasConversion<string>();
        }

        public DbSet<TodoItem> Todos { get; set; }

        public DbSet<Board> Boards { get; set; }

        public DbSet<BoardPermission> BoardPermissions { get; set; }

        public DbSet<UserBoard> UserBoards { get; set; }

        public DbSet<BoardShare> BoardShares { get; set; }
    }
}
