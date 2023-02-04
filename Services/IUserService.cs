using DataLibrary.Enums;
using DataLibrary.Models;
namespace ToDoListWithUsersApi.Services
{
    public interface IUserService
    {
        List<UserModel> GetAllUsers();

        UserModel GetUser(Guid userId);

        UserModel CreateUser(UserModel user);

        UserModel EditUser(UserModel newUser);

        UserModel PromoteUser(UserModel user);

        UserModel DemoteUser(UserModel user);

        UserModel ChangePassword(UserModel user);

        UserModel ChangeOtherPassword(UserModel user);

        UserModel DeleteUser(UserModel user);

        UserModel Login(UserModel user);

        UserModel? AuthenticateUser(string username, string password);

        UserModel Logout();

        UserModel UpdateSort(UserModel user);
    }
}
