using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Data;
using Todo.Service.Repositories;

namespace Todo.Service.UnitOfWorkRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoContext context;
       
        public UnitOfWork(TodoContext context)
        {
            this.context = context;
        }

        public IBoardRepository BoardRepository => new BoardRepository(context);
        public IUserRepository UserRepository => new UserRepository(context);
        public ITodoRepository TodoRepository => new TodoRepository(context);
        public IBoardSharedRepository BoardSharedRepository => new BoardSharedRepository(context);

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            context.ChangeTracker.DetectChanges();
            return context.ChangeTracker.HasChanges();
        }
    }
}
