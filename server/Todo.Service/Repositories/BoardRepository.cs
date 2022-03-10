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

        public async Task CreateBoardAsync(Board board, AppUser owner)
        {
            await todoContext.Boards.AddAsync(board);
            var permissions = await todoContext.BoardPermissions.SingleOrDefaultAsync(x => x.Permission == Permission.Admin);

            if (permissions == null)
                throw new NullReferenceException();

            var userBoards = new UserBoard
            {
                UserId = owner.Id,
                Board = board,
                BoardPermissionId = permissions.Id
            };

            await todoContext.UserBoards.AddAsync(userBoards);
        }

        public async Task<ICollection<Board>> GetBoardsByUserAsync(int userId)
        {
            return await todoContext.Boards
                .Where(b => b.UserBoards.Any(ub => ub.UserId == userId))
                .ToListAsync(); 
        }
    }
}
