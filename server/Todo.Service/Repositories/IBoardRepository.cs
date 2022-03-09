using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Service.Entities;
using Todo.Service.TodoDtos;

namespace Todo.Service.Repositories
{
    public interface IBoardRepository
    {
        Task CreateBoardAsync(Board board);
        Task<Board> GetBoardByIdAsync(int id);

        Task<ICollection<BoardDto>> GetBoardsByUserAsync(int userId);

        Task<bool> SaveChangeAsync();
    }
}