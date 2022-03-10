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
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext todoContext;
        public TodoRepository(TodoContext todoContext)
        {
            this.todoContext = todoContext;
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            return await todoContext.Todos.ToListAsync();
        }

        public async Task<TodoItem> GetTodoItemAsync(int id)
        {
            return await todoContext.Todos.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateTodoItemAsync(TodoItem todoItems)
        {
            await todoContext.AddAsync(todoItems);
        }
    }
}
