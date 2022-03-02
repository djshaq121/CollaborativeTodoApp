using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Data;
using Todo.Service.Entities;
using Todo.Service.TodoDtos;

namespace Todo.Service.Repositories
{
    public class TodoRepository
    {
        private readonly TodoContext todoContext;
        public TodoRepository(TodoContext todoContext)
        {
            this.todoContext = todoContext;
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItems()
        {
            return await todoContext.Todos.ToListAsync();
        }

        public async Task<TodoItem> GetTodoItem(int id)
        {
            return await todoContext.Todos.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateTodoItem(TodoItem todoItems)
        {
            todoContext.Add(todoItems);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await todoContext.SaveChangesAsync() > 0;
        }
    }
}
