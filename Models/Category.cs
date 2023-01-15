using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListWithUsersApi.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public SortTasks SortLists { get; set; } = SortTasks.New;
        public ICollection<TaskList> TaskLists { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public Category()
        {
            TaskLists = new List<TaskList>();
        }
    }
}
