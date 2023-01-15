using System.ComponentModel.DataAnnotations;

namespace ToDoListWithUsersApi.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Adress { get; set; }
        public PermissionLevel Permission { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public SortLists SortLists { get; set; } = SortLists.New;
        public SortCategories SortCategories { get; set; } = SortCategories.New;
        public ICollection<TaskList> TaskLists { get; set; }
        public ICollection<Category> Categories { get; set; }

        public User()
        {
            TaskLists = new List<TaskList>();
            Categories = new List<Category>();
        }
    }
}
