namespace ToDoListWithUsersApi.Services
{
    public interface ISubTaskService
    {
        List<SubTask> GetCurrentTaskSubTasks(Guid taskId);

        SubTask GetSingleSubTask(Guid id);

        SubTask CreateSubTask(Guid taskId, string title, string description);

        SubTask UpdateSubTask(Guid id, string? title, string? description);

        string DeleteSubTask(Guid id);
    }
}
