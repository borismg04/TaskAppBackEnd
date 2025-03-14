using Interfaces;
using Microsoft.AspNetCore.Http;
using Models;

namespace Services
{
    public class TaskService : ITaskService
    {
        private readonly DBManagement _context;
        private readonly string _secretKey;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;

        public TaskService(DBManagement context, string secretKey, IHttpContextAccessor httpContextAccessor, IAuthService authService)
        {
            _context = context;
            _secretKey = secretKey;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        public ReponseModel GetTasks(string? email, string? pass)
        {
            string error = string.Empty;
            DateTime time = DateTime.Now;
            try
            {
                var token = _authService.Authenticate(email, pass);
                _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", $"Bearer {token}");
                var tasks = _context.Tasks.ToList();
                if (tasks.Count == 0)
                {
                    return new ReponseModel
                    {
                        message = "No data found",
                        success = true,
                        result = tasks,
                        statusCode = 201
                    };
                }
                
                return new ReponseModel
                {
                    message = "Operation Success",
                    success = true,
                    result = tasks,
                    statusCode = 200
                };
            }
            catch (Exception ex)
            {
                var info = ErrorService.CatchService2("GetTasks", ex.Message, null, DateTime.Now);
                return new ReponseModel
                {
                    message = "Operation failed",
                    success = false,
                    result = info,
                    statusCode = 500
                };
            }
        }

        public ReponseModel CreateTask(string? email, string? pass, TaskModels task)
        {
            string error = string.Empty;
            DateTime time = DateTime.Now;
            try
            {
                var token = _authService.Authenticate(email, pass);
                _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", $"Bearer {token}");
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return new ReponseModel
                {
                    message = "Operation Success",
                    success = true,
                    result = task,
                    statusCode = 200
                };
            }
            catch (Exception ex)
            {
                error = "CreateTask: " + ex.Message;
                var info = ErrorService.CatchService2("CreateTask", error, null, time);
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
