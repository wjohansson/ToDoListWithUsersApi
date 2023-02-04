using DataLibrary;
using DataLibrary.Enums;
using DataLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ToDoListWithUsersApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly UserContext _dbContext;

        public TaskService(UserContext context)
        {
            _dbContext = context;
        }

        public TaskModel CreateTask(Guid listId, TaskModel task)
        {
            if (_dbContext.Tasks.Any(x => x.Title == task.Title && x.TaskListId == listId))
            {
                throw new Exception();
            }

            task.TaskListId = listId;

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            return task;
        }

        public TaskModel DeleteTask(TaskModel task)
        {
            TaskModel oldTask = _dbContext.Tasks.Include(x => x.SubTasks).First(u => u.Id == task.Id);

            _dbContext.Tasks.Remove(oldTask);
            _dbContext.SaveChanges();

            return oldTask;
        }

        public List<TaskModel> GetTasks()
        {
            var tasks = _dbContext.Tasks.Include(x => x.SubTasks).ToList();

            return tasks;
        }

        public List<TaskModel> GetCurrentListTasks()
        {
            return SortBy();
        }

        public TaskModel GetTask(Guid taskId)
        {
            CurrentActive.Id["TaskId"] = taskId;
            var task = _dbContext.Tasks.Include(x => x.SubTasks).First(x => x.Id == taskId);

            return task;
        }

        public TaskModel EditTask(TaskModel newTask)
        {
            TaskModel task = _dbContext.Tasks.Include(x => x.SubTasks).First(u => u.Id == newTask.Id);

            if (_dbContext.Tasks.Any(x => x.TaskListId == task.TaskListId && x.Title == newTask.Title && x.Title != task.Title))
            {
                throw new Exception();
            }

            task.Title = newTask.Title ?? task.Title;
            task.Description = newTask.Description ?? task.Description;
            task.Priority = newTask.Priority ?? task.Priority;

            _dbContext.SaveChanges();

            return task;
        }

        public TaskModel ToggleCompletion(TaskModel newTask)
        {
            TaskModel task = _dbContext.Tasks.Include(x => x.SubTasks).First(u => u.Id == newTask.Id);

            task.Completed = !task.Completed;

            _dbContext.SaveChanges();
            return task;
        }

        public TaskModel UpdateSort(TaskModel newTask)
        {
            TaskModel task = _dbContext.Tasks.Include(x => x.SubTasks).First(u => u.Id == newTask.Id);

            task.SortSubTasks = newTask.SortSubTasks;
            _dbContext.SaveChanges();

            return task;
        }

        public List<TaskModel> SortBy()
        {
            Guid listId = CurrentActive.Id["ListId"];
            SortTasks sortBy = _dbContext.TaskLists.First(u => u.Id == listId).SortTasks;
            List<TaskModel> currentListTasks = _dbContext.Tasks.Include(x => x.SubTasks).Where(x => x.TaskListId == listId).ToList();

            return sortBy switch
            {
                SortTasks.Name => currentListTasks.OrderBy(t => t.Title).ToList(),
                SortTasks.New => currentListTasks.OrderByDescending(t => t.DateCreated).ToList(),
                SortTasks.Old => currentListTasks.OrderBy(t => t.DateCreated).ToList(),
                SortTasks.Priority => currentListTasks.OrderByDescending(t => t.Priority).ToList(),
                SortTasks.Completion => currentListTasks.OrderByDescending(t => t.Completed).ToList(),
                _ => new List<TaskModel>(),
            };
        }
    }
}
