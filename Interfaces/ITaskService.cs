using Models;

namespace Interfaces
{
    public interface ITaskService
    {
        ReponseModel GetTasks(string? email, string? pass);
        ReponseModel CreateTask(string? email, string? pass, TaskModels task);
    }
}
