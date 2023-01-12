using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListWithUsersApi.Models
{
    public class SubTask
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("TaskId")]
        public Guid TaskId { get; set; }
    }
}
