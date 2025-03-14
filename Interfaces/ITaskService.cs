using Models;

namespace Interfaces
{
    public interface ITaskService
    {
        ReponseModel GetTasks(string? email, string? pass);
        ReponseModel CreateTask(string? email, string? pass, TaskModels task);
        ReponseModel UpdateTask(string? email, string? pass, TaskModels task);
        ReponseModel DeleteTask(string? email, string? pass, int id);
    }
}
