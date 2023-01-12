using Microsoft.EntityFrameworkCore;
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
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Completed = false,
                Priority = priority,
                DateCreated = DateTime.Now.ToString(),
                SubTasks = new List<SubTask>(),
                TaskListId = listId,
            };

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            return task;
        }

        public string DeleteTask(Guid id)
        {
            Task task = _dbContext.Tasks.First(u => u.Id == id);

            _dbContext.Tasks.Remove(task);
            _dbContext.SaveChanges();

            return "Task was deleted";
        }

        public List<Task> GetCurrentListTasks(Guid listId)
        {
            return _dbContext.Tasks.Where(t => t.TaskListId == listId).ToList();
        }

        //public List<Task> GetCurrentListTasks(Guid listId)
        //{

        //}

        public Task GetSingleTask(Guid id)
        {
            CurrentRecord.Record["TaskId"] = id.ToString();
            return _dbContext.Tasks.First(x => x.Id == id);
        }

        public Task UpdateTask(Guid id, string? title, string? description, bool? completed, Priority priority)
        {
            Task task = _dbContext.Tasks.First(u => u.Id == id);

            task.Title = title == null ? task.Title : title;
            task.Description = description == null ? task.Description : description;
            task.Completed = (bool)(completed == null ? task.Completed : completed);
            task.Priority = priority == null ? task.Priority : priority;

            _dbContext.SaveChanges();

            return task;
        }
    }
}
