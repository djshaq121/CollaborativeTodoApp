using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Todo.Service.Data;
using Todo.Service.Entities;

namespace Todo.Service.Repositories
{
    public class BoardSharedRepository : IBoardSharedRepository
    {
        private readonly TodoContext context;

        public BoardSharedRepository(TodoContext context)
        {
            this.context = context;
        }

        public async Task CreateBoardShare(BoardShare boardShare)
        {
            await context.BoardShares.AddAsync(boardShare);
        }

        public async Task<BoardShare> GetBoardShare(Expression<Func<BoardShare, bool>> expression)
        {
            return await context.BoardShares.SingleOrDefaultAsync(expression);
        }
    }
}
