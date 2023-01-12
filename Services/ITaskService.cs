using Task = ToDoListWithUsersApi.Models.Task;

namespace ToDoListWithUsersApi.Services
{
    public interface ITaskService
    {
        List<Task> GetCurrentListTasks(Guid listId);

        //List<Task> GetCurrentListTasks(Guid listId);

        Task GetSingleTask(Guid id);

        Task CreateTask(Guid listId, string title, string description, Priority priority);

        Task UpdateTask(Guid id, string? title, string? description, bool? completed, Priority priority);

        string DeleteTask(Guid id);
    }
}
