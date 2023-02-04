
using DataLibrary;
using DataLibrary.Enums;
using DataLibrary.Models;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ToDoListWithUsersApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _dbContext;

        public UserService(UserContext context)
        {
            _dbContext = context;
        }

        public List<UserModel> GetAllUsers()
        {
            var users = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).ToList();
            return users;
        }

        public UserModel GetUser(Guid userId)
        {
            var user = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).First(x => x.Id == userId);
            return user;
        }

        public UserModel CreateUser(UserModel user)
        {
            if (_dbContext.Users.Any(x => x.Username == user.Username))
            {
                throw new DuplicateNameException("Username already exists");
            }

            user.Id = Guid.NewGuid();
            user.Password = HashAndSaltPassword(user.Password, out var salt);
            user.OldPassword = "";
            user.ConfirmPassword = "";
            user.PasswordSalt = salt;
            user.Permission ??= PermissionLevel.User;

            _dbContext.Users.Add(user);

            CategoryModel standardCategory = new()
            {
                Title = "No category",
                UserId = user.Id
            };

            _dbContext.Categories.Add(standardCategory);

            _dbContext.SaveChanges();

            return user;
        }

        public UserModel EditUser(UserModel newUser)
        {
            UserModel user = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).First(u => u.Id == CurrentActive.Id["UserId"]);
        
            if (_dbContext.Users.Any(x => x.Username == newUser.Username && x.Username != user.Username))
            {
                throw new DuplicateNameException("Username already exists");
            }
            else if (user.Permission == PermissionLevel.System)
            {
                throw new InvalidOperationException("Can not change system user");
            }

            user.Username = newUser.Username ?? user.Username;

            user.FirstName = newUser.FirstName ?? user.FirstName;
            user.LastName = newUser.LastName ?? user.LastName;
            user.Email = newUser.Email ?? user.Email;
            user.Age = newUser.Age ?? user.Age;
            user.Gender = newUser.Gender ?? user.Gender;
            user.Adress = newUser.Adress ?? user.Adress;
            user.Permission = newUser.Permission ?? user.Permission;

            _dbContext.SaveChanges();

            return user;
        }

        public UserModel PromoteUser(UserModel newUser)
        {
            UserModel user = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).First(u => u.Id == newUser.Id);
            UserModel currentUserLoggedIn = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).First(u => u.Id == CurrentActive.Id["UserId"]);

            switch (user.Permission)
            {
                case PermissionLevel.System:
                    throw new InvalidOperationException("Can not change system user.");
                case PermissionLevel.Admin:
                    throw new InvalidOperationException("Can not promote to system.");
                case PermissionLevel.Moderator:
                    if (currentUserLoggedIn.Permission < PermissionLevel.Admin)
                    {
                        throw new InvalidOperationException("Insufficient privileges.");
                    }

                    break;
            }

            user.Permission += 1;
            _dbContext.SaveChanges();
            return user;
        }

        public UserModel DemoteUser(UserModel newUser)
        {
            UserModel user = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).First(u => u.Id == newUser.Id);
            UserModel currentUserLoggedIn = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).First(u => u.Id == CurrentActive.Id["UserId"]);

            switch (user.Permission)
            {
                case PermissionLevel.System:
                    throw new InvalidOperationException("Can not change system user.");
                case PermissionLevel.Admin:
                    if (currentUserLoggedIn.Permission < PermissionLevel.System)
                    {
                        throw new InvalidOperationException("Insufficient privileges.");
                    }

                    break;
                case PermissionLevel.Moderator:
                    if (currentUserLoggedIn.Permission < PermissionLevel.Admin)
                    {
                        throw new InvalidOperationException("Insufficient privileges.");
                    }

                    break;
                case PermissionLevel.User:
                    throw new InvalidOperationException("Can not demote from user.");
            }

            user.Permission -= 1;
            _dbContext.SaveChanges();
            return user;
        }

        public UserModel ChangePassword(UserModel user)
        {
            UserModel? oldUser = AuthenticateUser(user.Username, user.OldPassword);
            
            if (oldUser == null)
            {
                throw new UnauthorizedAccessException("Incorrect password");
            }
            else if (oldUser.Permission == PermissionLevel.System)
            {
                throw new InvalidOperationException("Can not change system user");
            }

            oldUser.Password = HashAndSaltPassword(user.Password, out var salt);
            oldUser.PasswordSalt = salt;
            oldUser.ConfirmPassword = "";

            _dbContext.SaveChanges();

            return oldUser;
        }

        public UserModel ChangeOtherPassword(UserModel user)
        {
            UserModel? oldUser = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).First(x => x.Id == user.Id);

            if (user.ConfirmPassword != user.Password)
            {
                throw new InvalidOperationException("Password do not match");
            }
            else if (oldUser.Permission == PermissionLevel.System)
            {
                throw new InvalidOperationException("Can not change system user");
            }

            oldUser.Password = HashAndSaltPassword(user.Password, out var salt);
            oldUser.PasswordSalt = salt;
            oldUser.ConfirmPassword = "";


            _dbContext.SaveChanges();

            return oldUser;
        }

        public UserModel DeleteUser(UserModel user)
        {
            UserModel oldUser = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).First(u => u.Id == user.Id);

            if (oldUser.Permission== PermissionLevel.System)
            {
                throw new UnauthorizedAccessException("Can not delete system user");
            }

            _dbContext.Users.Remove(oldUser);
            _dbContext.SaveChanges();

            return oldUser;
        }

        public UserModel Login(UserModel user)
        {
            if (user.Username == null || user.Password == null)
            {
                throw new InvalidOperationException("Username and password is required");
            }

            var checkUser = AuthenticateUser(user.Username, user.Password);

            if (checkUser == null)
            {
                throw new UnauthorizedAccessException("Incorrect username or password");
            }

            CurrentActive.Id["UserId"] = checkUser.Id;

            return user;
        }

        public UserModel? AuthenticateUser(string username, string password)
        {
            UserModel user;
            try
            {
                user = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).Single(x => x.Username == username);

                if (!VerifyPassword(password, user.Password, user.PasswordSalt) && password != user.Password)
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return user;
        }

        public string HashAndSaltPassword(string password, out byte[] salt)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string hash, byte[] salt)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }

        public UserModel Logout()
        {
            CurrentActive.Id.Clear();

            return new UserModel();
        }

        public UserModel UpdateSort(UserModel newUser)
        {
            UserModel user = _dbContext.Users.Include(x => x.TaskLists).Include(x => x.Categories).First(u => u.Id == newUser.Id);

            user.SortLists = newUser.SortLists;
            _dbContext.SaveChanges();

            return user;
        }
    }
}
