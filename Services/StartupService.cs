using DataLibrary.Enums;
using DataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoListWithUsersApi.Services
{
    public class StartupService
    {
        public void OnStarted()
        {
            var connectionString = "Server=localhost; port = 3306; username=root; password=DefaultPassword; database=ToDoList; Persist Security Info = false; Connect Timeout=300";

            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            using UserContext dbContext = new(optionsBuilder.Options);
            var userService = new UserService(dbContext);
            var categoryService = new CategoryService(dbContext);

            if (!dbContext.Users.Any(x => x.Permission == PermissionLevel.System))
            {
                UserModel systemUser = new()
                {
                    Id = Guid.NewGuid(),
                    Username = "System",
                    Password = userService.HashAndSaltPassword("SystemUser!1", out var salt),
                    OldPassword = "",
                    ConfirmPassword = "",
                    PasswordSalt = salt,
                    FirstName = "System",
                    LastName = "System",
                    Email = "system@mail.com",
                    Age = 9000,
                    Gender = "Robot",
                    Adress = "168.9.157.115",
                    Permission = PermissionLevel.System,
                };

                dbContext.Users.Add(systemUser);
            }

            foreach (var user in dbContext.Users.ToList())
            {
                if (!dbContext.Categories.Any(x => x.UserId == user.Id && x.Title == "No category"))
                {
                    CategoryModel category = new()
                    {
                        Title = "No category",
                        UserId = user.Id
                    };

                    dbContext.Categories.Add(category);
                }
            }
            
            dbContext.SaveChanges();
        }
    }
}
