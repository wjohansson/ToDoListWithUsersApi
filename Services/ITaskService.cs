using Task = ToDoListWithUsersApi.Models.Task;

namespace ToDoListWithUsersApi.Services
{
    public interface ITaskService
    {
        List<Task> GetTasks();

        List<Task> GetCurrentListTasks(Guid listId);

        Task GetTask(Guid taskId);

        Task CreateTask(Guid listId, string title, string description, Priority priority);

        Task EditTask(Guid taskId, string? title, string? description, Priority? priority);

        string DeleteTask(Guid taskId);

        string ToggleCompletion(Guid taskId);

        string UpdateSort(Guid taskId, SortSubTasks sortLists);

        List<Task> SortBy();
    }
}
