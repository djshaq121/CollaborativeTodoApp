using Todo.Service.Entities;

namespace Todo.Service.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);

        string CreateShareableToken();
    }
}