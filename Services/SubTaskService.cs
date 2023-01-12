using System.Collections.Generic;

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
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                TaskId = taskId,
            };

            _dbContext.SubTasks.Add(subTask);
            _dbContext.SaveChanges();

            return subTask;
        }

        public string DeleteSubTask(Guid id)
        {
            SubTask subTask = _dbContext.SubTasks.First(u => u.Id == id);

            _dbContext.SubTasks.Remove(subTask);
            _dbContext.SaveChanges();

            return "Sub Task was deleted";
        }

        public List<SubTask> GetCurrentTaskSubTasks(Guid taskId)
        {
            return _dbContext.SubTasks.Where(x => x.TaskId == taskId).ToList();
        }

        public SubTask GetSingleSubTask(Guid id)
        {
            CurrentRecord.Record["SubTaskId"] = id.ToString();

            return _dbContext.SubTasks.First(x => x.Id == id);
        }

        public SubTask UpdateSubTask(Guid id, string? title, string? description)
        {
            SubTask subTask = _dbContext.SubTasks.First(u => u.Id == id);

            subTask.Title = title == null ? subTask.Title : title;
            subTask.Description = description == null ? subTask.Description : description;

            _dbContext.SaveChanges();

            return subTask;
        }
    }
}
