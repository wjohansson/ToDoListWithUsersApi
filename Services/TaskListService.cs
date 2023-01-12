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
            var lists = GetAllLists();
            return lists.Where(x => x.UserId == userId).ToList();
        }
        
        public TaskList GetSingleList(Guid id)
        {
            CurrentRecord.Record["ListId"] = id.ToString();
            return _dbContext.TaskLists.First(x => x.Id == id);
        }

        public TaskList CreateList(Guid userId, string title, Guid categoryId)
        {
            TaskList taskList = new()
            {
                Id = Guid.NewGuid(),
                Title = title,
                CategoryId = categoryId,
                Tasks = new List<Task>(),
                UserId = userId
            };

            _dbContext.TaskLists.Add(taskList);
            _dbContext.SaveChanges();

            return taskList;
        }

        public TaskList UpdateList(Guid id, string? title, Guid categoryId)
        {
            TaskList taskList = _dbContext.TaskLists.First(u => u.Id == id);

            taskList.Title = title == null ? taskList.Title : title;
            taskList.CategoryId = categoryId == null ? taskList.CategoryId : categoryId;

            _dbContext.SaveChanges();

            return taskList;
        }

        public string DeleteList(Guid id)
        {
            TaskList taskList = _dbContext.TaskLists.First(u => u.Id == id);

            _dbContext.TaskLists.Remove(taskList);
            _dbContext.SaveChanges();

            return "Task list was deleted";
        }

    }
}
