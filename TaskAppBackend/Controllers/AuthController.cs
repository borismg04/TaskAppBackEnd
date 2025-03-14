using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;

namespace TaskAppBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ReponseModel result = _authService.RegisterUser(user);
            return StatusCode(result.statusCode, result);
        }
    }
}
