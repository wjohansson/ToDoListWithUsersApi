using DataLibrary;
using DataLibrary.Enums;
using DataLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoListWithUsersApi.Services
{
    public class SubTaskService : ISubTaskService
    {
        private readonly UserContext _dbContext;

        public SubTaskService(UserContext context)
        {
            _dbContext = context;
        }

        public SubTaskModel CreateSubTask(Guid taskId, SubTaskModel subTask)
        {
            if (_dbContext.SubTasks.Any(x => x.Title == subTask.Title && x.TaskId == taskId))
            {
                throw new Exception();
            }

            subTask.TaskId = taskId;

            _dbContext.SubTasks.Add(subTask);
            _dbContext.SaveChanges();

            return subTask;
        }

        public SubTaskModel DeleteSubTask(SubTaskModel subTask)
        {
            SubTaskModel oldSubTask = _dbContext.SubTasks.First(u => u.Id == subTask.Id);

            _dbContext.SubTasks.Remove(oldSubTask);
            _dbContext.SaveChanges();

            return oldSubTask;
        }

        public List<SubTaskModel> GetAllSubTasks()
        {
            return _dbContext.SubTasks.ToList();
        }

        public List<SubTaskModel> GetCurrentTaskSubTasks()
        {
            return SortBy();
        }
        
        public SubTaskModel GetSubTask(Guid subTaskId)
        {
            CurrentActive.Id["SubTaskId"] = subTaskId;

            return _dbContext.SubTasks.First(x => x.Id == subTaskId);
        }

        public SubTaskModel EditSubTask(SubTaskModel newSubTask)
        {
            SubTaskModel subTask = _dbContext.SubTasks.First(u => u.Id == newSubTask.Id);

            if (_dbContext.SubTasks.Any(x => x.TaskId == subTask.TaskId && x.Title == newSubTask.Title && x.Title != subTask.Title))
            {
                throw new Exception();
            }

            subTask.Title = newSubTask.Title ?? subTask.Title;
            subTask.Description = newSubTask.Description ?? subTask.Description;

            _dbContext.SaveChanges();

            return subTask;
        }

        public List<SubTaskModel> SortBy()
        {
            Guid taskId = CurrentActive.Id["TaskId"];
            SortSubTasks sortBy = _dbContext.Tasks.First(u => u.Id == taskId).SortSubTasks;
            List<SubTaskModel> currentTaskSubTasks = _dbContext.SubTasks.Where(x => x.TaskId == taskId).ToList();

            return sortBy switch
            {
                SortSubTasks.Name => currentTaskSubTasks.OrderBy(t => t.Title).ToList(),
                SortSubTasks.New => currentTaskSubTasks.OrderByDescending(t => t.DateCreated).ToList(),
                SortSubTasks.Old => currentTaskSubTasks.OrderBy(t => t.DateCreated).ToList(),
                _ => new List<SubTaskModel>(),
            };
        }
    }
}
