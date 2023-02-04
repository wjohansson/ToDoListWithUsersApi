using DataLibrary;
using DataLibrary.Enums;
using DataLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Security.Claims;

namespace ToDoListWithUsersApi.Services
{
    public class TaskListService : ITaskListService
    {
        private readonly UserContext _dbContext;

        public TaskListService(UserContext context)
        {
            _dbContext = context;
        }

        public List<TaskListModel> GetAllLists()
        {
            var lists = _dbContext.TaskLists.Include(x => x.Tasks).ToList();

            return lists;
        }

        public List<TaskListModel> GetCurrentUserLists()
        {
            return SortBy();
        }

        public List<TaskListModel> GetCurrentCategoryLists()
        {
            var lists = SortBy();
            return lists.Where(x => x.CategoryId == CurrentActive.Id["CategoryId"]).ToList();
        }

        public TaskListModel GetList(Guid listId)
        {
            CurrentActive.Id["ListId"] = listId;
            var taskList = _dbContext.TaskLists.Include(x => x.Tasks).First(x => x.Id == listId);

            return taskList;
        }

        public TaskListModel CreateList(Guid userId, TaskListModel taskList)
        {
            if (_dbContext.TaskLists.Any(x => x.Title == taskList.Title && x.UserId == userId))
            {
                throw new Exception();
            }

            if (taskList.CategoryId == null)
            {
                taskList.CategoryId = _dbContext.Categories.First(x => x.Title == "No category" && x.UserId == userId).Id;
            }

            taskList.UserId = userId; 

            _dbContext.TaskLists.Add(taskList);
            _dbContext.SaveChanges();

            return taskList;
        }

        public TaskListModel EditList(TaskListModel newTaskList)
        {
            TaskListModel taskList = _dbContext.TaskLists.Include(x => x.Tasks).First(u => u.Id == newTaskList.Id);

            if (_dbContext.TaskLists.Any(x => x.UserId == taskList.UserId && x.Title == newTaskList.Title && x.Title != taskList.Title))
            {
                throw new Exception();
            }

            taskList.Title = newTaskList.Title ?? taskList.Title;
            taskList.CategoryId = newTaskList.CategoryId != Guid.Empty ? newTaskList.CategoryId : taskList.CategoryId;

            _dbContext.SaveChanges();

            return taskList;
        }

        public TaskListModel DeleteList(TaskListModel taskList)
        {
            TaskListModel oldTaskList = _dbContext.TaskLists.Include(x => x.Tasks).First(u => u.Id == taskList.Id);

            _dbContext.TaskLists.Remove(oldTaskList);
            _dbContext.SaveChanges();

            return oldTaskList;
        }

        public TaskListModel UpdateSort(TaskListModel newTaskList)
        {
            TaskListModel list = _dbContext.TaskLists.Include(x => x.Tasks).First(u => u.Id == newTaskList.Id);

            list.SortTasks = newTaskList.SortTasks;
            _dbContext.SaveChanges();

            return list;
        }

        public List<TaskListModel> SortBy()
        {
            Guid userId = CurrentActive.Id["UserId"];
            SortLists sortBy = _dbContext.Users.First(u => u.Id == userId).SortLists;
            List<TaskListModel> currentUserLists = _dbContext.TaskLists.Include(x => x.Tasks).Where(x => x.UserId == userId).ToList();

            return sortBy switch
            {
                SortLists.Name => currentUserLists.OrderBy(t => t.Title).ToList(),
                SortLists.New => currentUserLists.OrderByDescending(t => t.DateCreated).ToList(),
                SortLists.Old => currentUserLists.OrderBy(t => t.DateCreated).ToList(),
                _ => new List<TaskListModel>(),
            };
        }
    }
}
