using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Service.Data;
using Todo.Service.Entities;

namespace Todo.Service.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly TodoContext todoContext;
        public BoardRepository(TodoContext todoContext)
        {
            this.todoContext = todoContext;
        }

        public async Task<Board> GetBoardByIdAsync(int id)
        {
            return await todoContext.Boards
                .Include(x => x.Todos)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateBoardAsync(Board board)
        {
            await todoContext.Boards.AddAsync(board);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await todoContext.SaveChangesAsync() > 0;
        }
    }
}
