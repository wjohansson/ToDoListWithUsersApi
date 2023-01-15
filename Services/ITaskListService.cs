using Task = ToDoListWithUsersApi.Models.Task;

namespace ToDoListWithUsersApi.Services
{
    public interface ITaskListService
    {
        List<TaskList> GetAllLists();

        List<TaskList> GetCurrentUserLists(Guid userId);

        List<TaskList> GetCurrentCategoryLists(Guid categoryId);

        TaskList GetList(Guid listId);

        TaskList CreateList(Guid userId, string title, Guid? categoryId);

        TaskList EditList(Guid listId, string? title, Guid? categoryId);

        string DeleteList(Guid listId);

        string UpdateSort(Guid listId, SortTasks sortBy);

        List<TaskList> SortBy();

    }
}
