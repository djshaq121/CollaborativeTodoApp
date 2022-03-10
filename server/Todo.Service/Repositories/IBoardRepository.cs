using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.TodoDtos;

namespace Todo.Service.Repositories
{
    public interface IBoardRepository
    {
        Task CreateBoardAsync(Board board, AppUser owner);
        Task<Board> GetBoardByIdAsync(int id);

        Task<ICollection<Board>> GetBoardsByUserAsync(int userId);

    }
}