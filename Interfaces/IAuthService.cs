using Models;
using Models.DTO;

namespace Interfaces
{
    public interface IAuthService
    {
        ReponseModel Authenticate(string email, string password);
        ReponseModel RegisterUser(UserModel user);

    }
}
