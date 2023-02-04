
using DataLibrary.Enums;
using DataLibrary.Models;

namespace ToDoListWithUsersApi.Services
{
    public interface ITaskService
    {
        List<TaskModel> GetTasks();

        List<TaskModel> GetCurrentListTasks();

        TaskModel GetTask(Guid taskId);

        TaskModel CreateTask(Guid listId, TaskModel task);

        TaskModel EditTask(TaskModel task);

        TaskModel DeleteTask(TaskModel task);

        TaskModel ToggleCompletion(TaskModel newTask);

        TaskModel UpdateSort(TaskModel task);

        List<TaskModel> SortBy();
    }
}
