namespace ToDoListWithUsersApi.Services
{
    public interface ITaskListService
    {
        List<TaskList> GetAllLists();

        List<TaskList> GetCurrentUserLists(Guid userId);

        TaskList GetSingleList(Guid id);

        TaskList CreateList(Guid userId, string title, Guid categoryId);

        TaskList UpdateList(Guid id, string? title, Guid categoryId);

        string DeleteList(Guid id);
    }
}
