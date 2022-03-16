using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Todo.Service.Entities;

namespace Todo.Service.Repositories
{
    public interface IBoardSharedRepository
    {
        Task CreateBoardShare(BoardShare boardShare);
        Task<BoardShare> GetBoardShare(Expression<Func<BoardShare, bool>> expression);
    }
}