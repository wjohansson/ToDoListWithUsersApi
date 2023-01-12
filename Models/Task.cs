using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListWithUsersApi.Models
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public Priority Priority { get; set; }
        public string DateCreated { get; init; }
        public ICollection<SubTask> SubTasks { get; set; }
        [ForeignKey("TaskListId")]
        public Guid TaskListId { get; set; }

        public Task() 
        {
            SubTasks = new List<SubTask>();
        }
    }
}
