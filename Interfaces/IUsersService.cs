using Models;
using Models.DTO;

namespace Interfaces
{
    public interface IUsersService
    {
        ReponseModel GetUsuarios(string? user , string? pass);
        ReponseModel UpdateUser(string? email, string? pass, int id, UserModel user);
        ReponseModel DeleteUser(string? email, string? pass, int id);
    }
}
