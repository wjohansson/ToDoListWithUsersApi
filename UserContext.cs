using DataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoListWithUsersApi
{
    public class UserContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<TaskListModel> TaskLists { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<SubTaskModel> SubTasks { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    }
}
