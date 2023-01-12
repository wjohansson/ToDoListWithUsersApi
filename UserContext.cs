using Microsoft.EntityFrameworkCore;
using Task = ToDoListWithUsersApi.Models.Task;

namespace ToDoListWithUsersApi
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    }
}
