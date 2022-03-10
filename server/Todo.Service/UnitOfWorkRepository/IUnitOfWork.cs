using System.Threading.Tasks;
using Todo.Service.Repositories;

namespace Todo.Service.UnitOfWorkRepository
{
    public interface IUnitOfWork
    {
        IBoardRepository BoardRepository { get; }
        IUserRepository UserRepository { get; }
        ITodoRepository TodoRepository { get; }

        bool HasChanges();
        Task<bool> SaveAsync();
    }
}