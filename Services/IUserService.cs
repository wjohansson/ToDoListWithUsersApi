namespace ToDoListWithUsersApi.Services
{
    public interface IUserService
    {
        List<User> GetAllUsers();

        User GetSingleUser(Guid id);

        User CreateUser(string username, string password, string firstName, string lastName, string email, int age, string gender, string adress, PermissionLevel permission);

        User UpdateUser(Guid id, string? username, string? password, string? firstName, string? lastName, string? email, int? age, string? gender, string? adress, PermissionLevel permission);

        string DeleteUser(Guid id);
        //public string Login(string username, string password);

        Task<string> Login(string? username, string? password);
        Task<User> AuthenticateUser(string username, string password);

        string Logout();
    }
}
