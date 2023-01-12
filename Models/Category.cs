using System.ComponentModel.DataAnnotations;

namespace ToDoListWithUsersApi.Models
{
    public class Category
    {
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();
        public string CategoryTitle { get; set; }
    }
}
