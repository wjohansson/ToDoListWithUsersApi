
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoListWithUsersApi.Models;

namespace ToDoListWithUsersApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _dbContext;

        public UserService(UserContext context)
        {
            _dbContext = context;
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public User GetUser(Guid userId)
        {
            return _dbContext.Users.First(x => x.Id == userId);
        }

        public User CreateUser(string username, string password, string firstName, string lastName, string email, int age, string gender, string adress, PermissionLevel? permission)
        {
            permission ??= PermissionLevel.User;

            User user = new()
            {
                Username = username,
                Password = HashAndSaltPassword(password, out var salt),
                PasswordSalt = salt,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Age = age,
                Gender = gender,
                Adress = adress,
                Permission = (PermissionLevel) permission,
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }

        public User EditUser(Guid userId, string? username, string? password, string? firstName, string? lastName, string? email, int? age, string? gender, string? adress, PermissionLevel? permission)
        {
            User user = _dbContext.Users.First(u => u.Id == userId);

            user.Username = username ?? user.Username;

            string? newPassword = null;
            byte[]? newSalt = null;
            if (password != null)
            {
                newPassword = HashAndSaltPassword(password, out var salt);
                newSalt = salt;
            }
            user.Password = newPassword ?? user.Password;
            user.PasswordSalt = newSalt ?? user.PasswordSalt;
            user.FirstName = firstName ?? user.FirstName;
            user.LastName = lastName ?? user.LastName;
            user.Email = email ?? user.Email;
            user.Age = age ?? user.Age;
            user.Gender = gender ?? user.Gender;
            user.Adress = adress ?? user.Adress;
            user.Permission = permission ?? user.Permission;

            _dbContext.SaveChanges();

            return user;
        }

        public User PromoteUser(Guid userId)
        {
            User user = _dbContext.Users.First(u => u.Id == userId);
            User currentUserLoggedIn = _dbContext.Users.First(u => u.Id == Guid.Parse(CurrentRecord.Record["UserId"]));

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

        public User DemoteUser(Guid userId)
        {
            User user = _dbContext.Users.First(u => u.Id == userId);
            User currentUserLoggedIn = _dbContext.Users.First(u => u.Id == Guid.Parse(CurrentRecord.Record["UserId"]));

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

        public string DeleteUser(Guid userId)
        {
            User user = _dbContext.Users.First(u => u.Id == userId);

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return "User was deleted";
        }

        public async Task<string> Login(string? username, string? password)
        {
            if (username == null || password == null)
            {
                throw new InvalidOperationException();
            }

            User? user = await AuthenticateUser(username, password);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            CurrentRecord.Record["UserId"] = user.Id.ToString();

            //var userId = user.Id;

            //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecretkey@123"));

            //var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(60)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public async Task<User?> AuthenticateUser(string username, string password)
        {
            User user;
            try
            {
                user = _dbContext.Users.Single(x => x.Username == username);

                if (!VerifyPassword(password, user.Password, user.PasswordSalt))
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

        public string Logout()
        {
            CurrentRecord.Record.Clear();

            return "Logged out.";
        }

        public string UpdateSort(Guid userId, SortLists sortBy)
        {
            User user = _dbContext.Users.First(u => u.Id == userId);

            user.SortLists = sortBy;
            _dbContext.SaveChanges();

            return "'Sort by' type was updated";
        }
    }
}
