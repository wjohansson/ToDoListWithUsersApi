using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security;
using System.Security.Claims;
using ToDoListWithUsersApi.Models;
using Task = ToDoListWithUsersApi.Models.Task;

namespace ToDoListWithUsersApi.Services
{
    public class TaskListService : ITaskListService
    {
        private readonly UserContext _dbContext;

        public TaskListService(UserContext context)
        {
            _dbContext = context;
        }

        public List<TaskList> GetAllLists()
        {
            return _dbContext.TaskLists.ToList();
        }

        public List<TaskList> GetCurrentUserLists(Guid userId)
        {
            return SortBy();
        }

        public List<TaskList> GetCurrentCategoryLists(Guid categoryId)
        {
            var lists = SortBy();
            return lists.Where(x => x.CategoryId == categoryId).ToList();
        }

        public TaskList GetList(Guid listId)
        {
            CurrentRecord.Record["ListId"] = listId.ToString();
            return _dbContext.TaskLists.First(x => x.Id == listId);
        }

        public TaskList CreateList(Guid userId, string title, Guid? categoryId)
        {
            TaskList taskList = new()
            {
                Title = title,
                CategoryId = categoryId ?? null,
                UserId = userId
            };

            _dbContext.TaskLists.Add(taskList);
            _dbContext.SaveChanges();

            return taskList;
        }

        public TaskList EditList(Guid listId, string? title, Guid? categoryId)
        {
            TaskList taskList = _dbContext.TaskLists.First(u => u.Id == listId);

            taskList.Title = title ?? taskList.Title;
            taskList.CategoryId = categoryId ?? taskList.CategoryId;

            _dbContext.SaveChanges();

            return taskList;
        }

        public string DeleteList(Guid listId)
        {
            TaskList taskList = _dbContext.TaskLists.First(u => u.Id == listId);

            _dbContext.TaskLists.Remove(taskList);
            _dbContext.SaveChanges();

            return "Task list was deleted";
        }

        public string UpdateSort(Guid listId, SortTasks sortBy)
        {
            TaskList list = _dbContext.TaskLists.First(u => u.Id == listId);

            list.SortTasks = sortBy;
            _dbContext.SaveChanges();

            return "'Sort by' type was updated";
        }

        public List<TaskList> SortBy()
        {
            Guid userId = Guid.Parse(CurrentRecord.Record["UserId"]);
            SortLists sortBy = _dbContext.Users.First(u => u.Id == userId).SortLists;
            List<TaskList> currentUserLists = _dbContext.TaskLists.Where(x => x.UserId == userId).ToList();

            return sortBy switch
            {
                SortLists.Name => currentUserLists.OrderBy(t => t.Title).ToList(),
                SortLists.New => currentUserLists.OrderByDescending(t => t.DateCreated).ToList(),
                SortLists.Old => currentUserLists.OrderBy(t => t.DateCreated).ToList(),
                SortLists.Category => currentUserLists.OrderBy(t => t.CategoryId).ToList(),
                _ => new List<TaskList>(),
            };
        }
    }
}
