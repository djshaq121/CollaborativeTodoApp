using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Data;

namespace Todo.Service.Repositories
{
    public class Repository<T> where T: class
    {
        private readonly TodoContext todoContext;
        private readonly DbSet<T> dbSet;
        public Repository(TodoContext todoContext)
        {
            this.todoContext = todoContext;
            this.dbSet = todoContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        //public async Task<IEnumerable<T>> GetAllAsync(Func<T, bool> filter)
        //{
        //    await todoContext.Todos.Where(x => x.Id ==1).ToListAsync()
        //    return await dbSet.Where(x => x.).ToListAsync();
        //}
    }
}
