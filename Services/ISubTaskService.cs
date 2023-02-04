using DataLibrary.Models;

namespace ToDoListWithUsersApi.Services
{
    public interface ISubTaskService
    {
        List<SubTaskModel> GetAllSubTasks();

        List<SubTaskModel> GetCurrentTaskSubTasks();

        SubTaskModel GetSubTask(Guid subTaskId);

        SubTaskModel CreateSubTask(Guid taskId, SubTaskModel subTask);

        SubTaskModel EditSubTask(SubTaskModel newSubTask);

        SubTaskModel DeleteSubTask(SubTaskModel subTask);

        List<SubTaskModel> SortBy();
    }
}
