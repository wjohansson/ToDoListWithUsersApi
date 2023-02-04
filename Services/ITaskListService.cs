using DataLibrary.Enums;
using DataLibrary.Models;

namespace ToDoListWithUsersApi.Services
{
    public interface ITaskListService
    {
        List<TaskListModel> GetAllLists();

        List<TaskListModel> GetCurrentUserLists();

        List<TaskListModel> GetCurrentCategoryLists();

        TaskListModel GetList(Guid listId);

        TaskListModel CreateList(Guid userId, TaskListModel taskList);

        TaskListModel EditList(TaskListModel newTaskList);

        TaskListModel DeleteList(TaskListModel taskList);

        TaskListModel UpdateSort(TaskListModel taskList);

        List<TaskListModel> SortBy();

    }
}
