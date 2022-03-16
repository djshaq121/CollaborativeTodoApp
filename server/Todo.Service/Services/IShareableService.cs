using System.Threading.Tasks;
using Todo.Service.Entities;

namespace Todo.Service.Services
{
    public interface IShareableService
    {
        Task<BoardShare> MakeBoardSharable(AppUser user, Board board);

        Task<bool> AddUserToBoard(AppUser user, string token);
    }
}