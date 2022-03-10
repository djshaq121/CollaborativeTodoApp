using System.Threading.Tasks;
using Todo.Service.Entities;

namespace Todo.Service.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
    }
}