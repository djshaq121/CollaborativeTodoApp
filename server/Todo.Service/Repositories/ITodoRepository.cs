using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Service.Entities;

namespace Todo.Service.Repositories
{
    public interface ITodoRepository
    {
        Task CreateTodoItemAsync(TodoItem todoItems);
        Task<TodoItem> GetTodoItemAsync(int id);
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync();
        Task<bool> SaveChangeAsync();
    }
}