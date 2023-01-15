using System.Collections.Generic;
using Task = ToDoListWithUsersApi.Models.Task;

namespace ToDoListWithUsersApi.Services
{
    public class SubTaskService : ISubTaskService
    {
        private readonly UserContext _dbContext;

        public SubTaskService(UserContext context)
        {
            _dbContext = context;
        }

        public SubTask CreateSubTask(Guid taskId, string title, string description)
        {
            SubTask subTask = new()
            {
                Title = title,
                Description = description,
                TaskId = taskId,
            };

            _dbContext.SubTasks.Add(subTask);
            _dbContext.SaveChanges();

            return subTask;
        }

        public string DeleteSubTask(Guid subTaskId)
        {
            SubTask subTask = _dbContext.SubTasks.First(u => u.Id == subTaskId);

            _dbContext.SubTasks.Remove(subTask);
            _dbContext.SaveChanges();

            return "Sub Task was deleted";
        }

        public List<SubTask> GetAllSubTasks()
        {
            return _dbContext.SubTasks.ToList();
        }

        public List<SubTask> GetCurrentTaskSubTasks(Guid taskId)
        {
            return SortBy();
        }
        
        public SubTask GetSingleSubTask(Guid subTaskId)
        {
            CurrentRecord.Record["SubTaskId"] = subTaskId.ToString();

            return _dbContext.SubTasks.First(x => x.Id == subTaskId);
        }

        public SubTask UpdateSubTask(Guid subTaskId, string? title, string? description)
        {
            SubTask subTask = _dbContext.SubTasks.First(u => u.Id == subTaskId);

            subTask.Title = title ?? subTask.Title;
            subTask.Description = description ?? subTask.Description;

            _dbContext.SaveChanges();

            return subTask;
        }

        public List<SubTask> SortBy()
        {
            Guid taskId = Guid.Parse(CurrentRecord.Record["TaskId"]);
            SortSubTasks sortBy = _dbContext.Tasks.First(u => u.Id == taskId).SortSubTasks;
            List<SubTask> currentTaskSubTasks = _dbContext.SubTasks.Where(x => x.TaskId == taskId).ToList();

            switch (sortBy)
            {
                case SortSubTasks.Name:
                    return currentTaskSubTasks.OrderBy(t => t.Title).ToList();
                case SortSubTasks.New:
                    return currentTaskSubTasks.OrderByDescending(t => t.DateCreated).ToList();
                case SortSubTasks.Old:
                    return currentTaskSubTasks.OrderBy(t => t.DateCreated).ToList();
            }
            return new List<SubTask>();
        }
    }
}
