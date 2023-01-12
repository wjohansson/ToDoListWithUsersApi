using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListWithUsersApi.Models
{
    public class TaskList
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        [ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; }
        public ICollection<Task> Tasks { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public TaskList()
        {
            Tasks = new List<Task>();
        }


    }
}
