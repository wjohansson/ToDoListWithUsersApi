using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ToDoListWithUsersApi.Models;
using Task = ToDoListWithUsersApi.Models.Task;

namespace ToDoListWithUsersApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly UserContext _dbContext;

        public TaskService(UserContext context)
        {
            _dbContext = context;
        }

        public Task CreateTask(Guid listId, string title, string description, Priority priority)
        {
            Task task = new()
            {
                Title = title,
                Description = description,
                Priority = priority,
                TaskListId = listId,
            };

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            return task;
        }

        public string DeleteTask(Guid taskId)
        {
            Task task = _dbContext.Tasks.First(u => u.Id == taskId);

            _dbContext.Tasks.Remove(task);
            _dbContext.SaveChanges();

            return "Task was deleted";
        }

        public List<Task> GetTasks()
        {
            return _dbContext.Tasks.ToList();
        }

        public List<Task> GetCurrentListTasks(Guid listId)
        {
            return SortBy();
        }

        public Task GetTask(Guid taskId)
        {
            CurrentRecord.Record["TaskId"] = taskId.ToString();
            return _dbContext.Tasks.First(x => x.Id == taskId);
        }

        public Task EditTask(Guid taskId, string? title, string? description, Priority? priority)
        {
            Task task = _dbContext.Tasks.First(u => u.Id == taskId);

            task.Title = title ?? task.Title;
            task.Description = description ?? task.Description;
            task.Priority = priority ?? task.Priority;

            _dbContext.SaveChanges();

            return task;
        }

        public string ToggleCompletion(Guid taskId)
        {
            Task task = _dbContext.Tasks.First(u => u.Id == taskId);

            task.Completed = !task.Completed;

            _dbContext.SaveChanges();
            return "Task completion toggled";
        }

        public string UpdateSort(Guid taskId, SortSubTasks sortBy)
        {
            Task task = _dbContext.Tasks.First(u => u.Id == taskId);

            task.SortSubTasks = sortBy;
            _dbContext.SaveChanges();

            return "'Sort by' type was updated";
        }

        public List<Task> SortBy()
        {
            Guid listId = Guid.Parse(CurrentRecord.Record["ListId"]);
            SortTasks sortBy = _dbContext.TaskLists.First(u => u.Id == listId).SortTasks;
            List<Task> currentListTasks = _dbContext.Tasks.Where(x => x.TaskListId == listId).ToList();

            return sortBy switch
            {
                SortTasks.Name => currentListTasks.OrderBy(t => t.Title).ToList(),
                SortTasks.New => currentListTasks.OrderByDescending(t => t.DateCreated).ToList(),
                SortTasks.Old => currentListTasks.OrderBy(t => t.DateCreated).ToList(),
                SortTasks.Priority => currentListTasks.OrderByDescending(t => t.Priority).ToList(),
                SortTasks.Completion => currentListTasks.OrderByDescending(t => t.Completed).ToList(),
                _ => new List<Task>(),
            };
        }
    }
}
