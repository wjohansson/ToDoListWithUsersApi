namespace ToDoListWithUsersApi.Services
{
    public interface ISubTaskService
    {
        List<SubTask> GetAllSubTasks();

        List<SubTask> GetCurrentTaskSubTasks(Guid taskId);

        SubTask GetSingleSubTask(Guid subTaskId);

        SubTask CreateSubTask(Guid taskId, string title, string description);

        SubTask UpdateSubTask(Guid subTaskId, string? title, string? description);

        string DeleteSubTask(Guid subTaskId);

        List<SubTask> SortBy();
    }
}
