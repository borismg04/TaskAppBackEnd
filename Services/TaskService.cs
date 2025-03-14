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

        //getTasks
        public ReponseModel GetTasks(string? email, string? pass)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                return new ReponseModel
                {
                    message = "Email and password are required.",
                    success = false,
                    result = null,
                    statusCode = 400
                };
            }

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


        // CreateTask
        public ReponseModel CreateTask(string? email, string? pass, TaskModels task)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                return new ReponseModel
                {
                    message = "Email and password are required.",
                    success = false,
                    result = null,
                    statusCode = 400
                };
            }

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

        public ReponseModel UpdateTask(string? email, string? pass, TaskModels task)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                return new ReponseModel
                {
                    message = "Email and password are required.",
                    success = false,
                    result = null,
                    statusCode = 400
                };
            }

            string error = string.Empty;
            DateTime time = DateTime.Now;
            try
            {
                var token = _authService.Authenticate(email, pass);
                if (token.result == null)
                {
                    return new ReponseModel
                    {
                        message = "Undefined",
                        success = false,
                        result = null,
                        statusCode = 401
                    };
                }
                _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", $"Bearer {token}");
                var taskUpdate = _context.Tasks.FirstOrDefault(x => x.Id == task.Id);
                if (taskUpdate == null)
                {
                    return new ReponseModel
                    {
                        message = "No data found",
                        success = false,
                        result = null,
                        statusCode = 201
                    };
                }
                taskUpdate.Nombre_Tarea = task.Nombre_Tarea;
                taskUpdate.Status = task.Status;
                taskUpdate.Descripcion = task.Descripcion;
                taskUpdate.Fecha = task.Fecha;
                taskUpdate.UserGestion = task.UserGestion;
                taskUpdate.Prioridad = task.Prioridad;
                _context.SaveChanges();
                return new ReponseModel
                {
                    message = "Operation Success",
                    success = true,
                    result = taskUpdate,
                    statusCode = 200
                };
            }
            catch (Exception ex)
            {
                error = "UpdateTask: " + ex.Message;
                var info = ErrorService.CatchService2("UpdateTask", error, null, time);
                return new ReponseModel
                {
                    message = "Operation Failed",
                    success = false,
                    result = info,
                    statusCode = 500
                };
            }
        }

        public ReponseModel DeleteTask(string? email, string? pass, int id)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                return new ReponseModel
                {
                    message = "Email and password are required.",
                    success = false,
                    result = null,
                    statusCode = 400
                };
            }

            string error = string.Empty;
            DateTime time = DateTime.Now;
            try
            {
                var token = _authService.Authenticate(email, pass);

                if (token.result == null)
                {
                    return new ReponseModel
                    {
                        message = "Undefined",
                        success = false,
                        result = null,
                        statusCode = 401
                    };
                }

                if (token.result == null)
                {
                    return new ReponseModel
                    {
                        message = "Undefined",
                        success = false,
                        result = null,
                        statusCode = 401
                    };
                }
                _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", $"Bearer {token}");
                var taskDelete = _context.Tasks.FirstOrDefault(x => x.Id == id);
                if (taskDelete == null)
                {
                    return new ReponseModel
                    {
                        message = "No data found",
                        success = false,
                        result = null,
                        statusCode = 201
                    };
                }
                _context.Tasks.Remove(taskDelete);
                _context.SaveChanges();
                return new ReponseModel
                {
                    message = "Operation Success",
                    success = true,
                    result = taskDelete,
                    statusCode = 200
                };
            }
            catch (Exception ex)
            {
                error = "DeleteTask: " + ex.Message;
                var info = ErrorService.CatchService2("DeleteTask", error, null, time);
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
