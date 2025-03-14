using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace TaskAppBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly DBManagement _context;

        public TaskController(ITaskService taskService, DBManagement context)
        {
            _taskService = taskService;
            _context = context;
        }

        [HttpGet]
        [Route("GetTasks")]
        public IActionResult GetTasks()
        {
            string email = Request.Headers["email"];
            string pass = String.IsNullOrEmpty(Request.Headers["pass"]) ? "No aplica" : Request.Headers["pass"];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ReponseModel result = _taskService.GetTasks(email, pass);
            return StatusCode(result.statusCode, result);
        }

        [Route("CreateTask")]
        [HttpPost]
        public IActionResult CreateTask(TaskModels task)
        {
            string email = Request.Headers["email"];
            string pass = String.IsNullOrEmpty(Request.Headers["pass"]) ? "No aplica" : Request.Headers["pass"];
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ReponseModel result = _taskService.CreateTask(email, pass, task);
            return StatusCode(result.statusCode, result);
        }
    }
}
