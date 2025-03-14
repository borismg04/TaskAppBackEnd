using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;

namespace TaskAppBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBManagement _context;
        private readonly IUsersService _usersService;


        public UsersController(DBManagement context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        [HttpGet]
        [Route("GetUsuarios")]
        public IActionResult GetUsuarios()
        {
            string email = Request.Headers["email"];
            string pass = String.IsNullOrEmpty(Request.Headers["pass"]) ? "No aplica" : Request.Headers["pass"];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ReponseModel result = _usersService.GetUsuarios(email, pass);
            return StatusCode(result.statusCode, result);
        }

        [Route("UpdateUser")]
        [HttpPost]
        public IActionResult UpdateUser(int id, UserModel user)
        {
            string email = Request.Headers["email"];
            string pass = String.IsNullOrEmpty(Request.Headers["pass"]) ? "No aplica" : Request.Headers["pass"];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ReponseModel result = _usersService.UpdateUser(email, pass, id, user);
            return StatusCode(result.statusCode, result);
        }

        [Route("DeleteUser")]
        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            string email = Request.Headers["email"];
            string pass = String.IsNullOrEmpty(Request.Headers["pass"]) ? "No aplica" : Request.Headers["pass"];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ReponseModel result = _usersService.DeleteUser(email, pass, id);
            return StatusCode(result.statusCode, result);
        }
    }
}