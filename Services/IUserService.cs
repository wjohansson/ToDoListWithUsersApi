namespace ToDoListWithUsersApi.Services
{
    public interface IUserService
    {
        List<User> GetAllUsers();

        User GetUser(Guid userId);

        User CreateUser(string username, string password, string firstName, string lastName, string email, int age, string gender, string adress, PermissionLevel? permission);

        User EditUser(Guid userId, string? username, string? password, string? firstName, string? lastName, string? email, int? age, string? gender, string? adress, PermissionLevel? permission);

        string DeleteUser(Guid userId);

        Task<string> Login(string? username, string? password);

        Task<User?> AuthenticateUser(string username, string password);

        string Logout();

        string UpdateSort(Guid userId, SortLists sortLists);
    }
}
