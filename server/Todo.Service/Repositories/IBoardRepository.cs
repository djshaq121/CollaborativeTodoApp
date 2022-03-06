using System.Threading.Tasks;
using Todo.Service.Entities;

namespace Todo.Service.Repositories
{
    public interface IBoardRepository
    {
        Task CreateBoardAsync(Board board);
        Task<Board> GetBoardByIdAsync(int id);
        Task<bool> SaveChangeAsync();
    }
}