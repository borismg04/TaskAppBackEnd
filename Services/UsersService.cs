using Interfaces;
using Microsoft.AspNetCore.Http;
using Models;
using Models.DTO;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly DBManagement _context;
        private readonly string _secretKey;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;

        public UsersService(DBManagement context, string secretKey, IHttpContextAccessor httpContextAccessor, IAuthService authService)
        {
            _context = context;
            _secretKey = secretKey;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        public ReponseModel GetUsuarios(string? email, string? pass)
        {
            string error = string.Empty;
            DateTime time = DateTime.Now;
            try
            {
                var token = _authService.Authenticate(email, pass);
                _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", $"Bearer {token}");

                var users = _context.Users.ToList();

                if (users.Count == 0)
                {
                    return new ReponseModel
                    {
                        message = "No data found",
                        success = true,
                        result = new List<UserResponseDTO>(),
                        statusCode = 201
                    };
                }

                var userDTOs = users.Select(user => new UserResponseDTO
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Email = user.Email,
                    Profile = user.Profile
                }).ToList();

                return new ReponseModel
                {
                    message = "Operation Success",
                    success = true,
                    result = userDTOs,
                    statusCode = 200
                };
            }
            catch (Exception ex)
            {
                error = "GetUsuarios: " + ex.Message;
                var info = ErrorService.CatchService2("GetUsuarios", error, null, time);
                return new ReponseModel
                {
                    message = "Operation Failed",
                    success = false,
                    result = info,
                    statusCode = 500
                };
            }
        }

        public ReponseModel UpdateUser(string? email, string? pass, int id, UserModel user)
        {
            string error = string.Empty;
            DateTime time = DateTime.Now;
            try
            {
                var token = _authService.Authenticate(email, pass);
                _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", $"Bearer {token}");

                var userToUpdate = _context.Users.FirstOrDefault(x => x.Id == user.Id);

                if (userToUpdate == null)
                {
                    return new ReponseModel
                    {
                        message = "User not found",
                        success = false,
                        result = null,
                        statusCode = 404
                    };
                }

                userToUpdate.Nombre = user.Nombre;
                userToUpdate.Email = user.Email;
                userToUpdate.Profile = user.Profile;

                _context.Users.Update(userToUpdate);
                _context.SaveChanges();

                var userResponse = new UserResponseDTO
                {
                    Id = userToUpdate.Id,
                    Nombre = userToUpdate.Nombre,
                    Email = userToUpdate.Email,
                    Profile = userToUpdate.Profile
                };

                return new ReponseModel
                {
                    message = "User updated",
                    success = true,
                    result = userResponse,
                    statusCode = 200
                };
            }
            catch (Exception ex)
            {
                error = "UpdateUser: " + ex.Message;
                var info = ErrorService.CatchService2("UpdateUser", error, null, time);
                return new ReponseModel
                {
                    message = "Operation Failed",
                    success = false,
                    result = info,
                    statusCode = 500
                };
            }
        }

        public ReponseModel DeleteUser(string? email, string? pass, int id)
        {
            string error = string.Empty;
            DateTime time = DateTime.Now;
            try
            {
                var UserId = _context.Users.Find(id);

                if (UserId == null)
                {
                    return new ReponseModel
                    {
                        message = "User not found",
                        success = false,
                        result = null,
                        statusCode = 201
                    };
                }

                _context.Users.Remove(UserId);
                _context.SaveChanges();


                var userResponse = new UserResponseDTO
                {
                    Id = UserId.Id,
                    Nombre = UserId.Nombre,
                    Email = UserId.Email,
                    Profile = UserId.Profile
                };

                return new ReponseModel
                {
                    message = "User deleted",
                    success = true,
                    result = userResponse,
                    statusCode = 200
                };
            }
            catch (Exception ex)
            {
                error = "DeleteUser: " + ex.Message;
                var info = ErrorService.CatchService2("DeleteUser", error, null, time);
                return new ReponseModel
                {
                    message = "Operation Failed",
                    success = false,
                    result = info,
                    statusCode = 500
                };
            }
        }
    }
}
