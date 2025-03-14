using Microsoft.IdentityModel.Tokens;
using Models;
using Interfaces;
using Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly DBManagement _context;
        private readonly string _secretKey;

        public AuthService(DBManagement context, string secretKey)
        {
            _context = context;
            _secretKey = secretKey;
        }

        public ReponseModel Authenticate(string email, string password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return new ReponseModel
                    {
                        message = "Invalid credentials",
                        success = false,
                        result = null,
                        statusCode = 401
                    };
                }

                var token = GenerateJwtToken(user);

                return new ReponseModel
                {
                    message = "Authentication successful",
                    success = true,
                    result = new { Token = token },
                    statusCode = 200
                };
            }
            catch (Exception ex)
            {
                var info = ErrorService.CatchService2("Authenticate", ex.Message, null, DateTime.Now);
                return new ReponseModel
                {
                    message = "Authentication failed",
                    success = false,
                    result = info,
                    statusCode = 500
                };
            }
        }

        // Register user
        public ReponseModel RegisterUser(UserModel user)
        {
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                return new ReponseModel
                {
                    message = "User registered",
                    success = true,
                    result = user,
                    statusCode = 200
                };
            }
            catch (Exception ex)
            {
                var info = ErrorService.CatchService2("RegisterUser", ex.Message, null, DateTime.Now);
                return new ReponseModel
                {
                    message = "Registration failed",
                    success = false,
                    result = info,
                    statusCode = 500
                };
            }
        }

        private string GenerateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email!)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
