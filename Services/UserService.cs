
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

        public User GetSingleUser(Guid id)
        {
            return _dbContext.Users.First(x => x.Id == id);
        }

        public User CreateUser(string username, string password, string firstName, string lastName, string email, int age, string gender, string adress, PermissionLevel permission)
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = HashAndSaltPassword(password, out var salt),
                PasswordSalt = salt,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Age = age,
                Gender = gender,
                Adress = adress,
                Permission = permission,
                TaskLists = new List<TaskList>()
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }

        public User UpdateUser(Guid id, string? username, string? password, string? firstName, string? lastName, string? email, int? age, string? gender, string? adress, PermissionLevel permission)
        {
            User user = _dbContext.Users.First(u => u.Id == id);

            user.Username = username == null ? user.Username : username;

            string newPassword = "";
            byte[] newSalt = new byte[0];
            if (password != null)
            {
                newPassword = HashAndSaltPassword(password, out var salt);
                newSalt = salt;
            }
            user.Password = password == null ? user.Password : newPassword;
            user.PasswordSalt = password == null ? user.PasswordSalt : newSalt;
            user.FirstName = firstName == null ? user.FirstName : firstName;
            user.LastName = lastName == null ? user.LastName : lastName;
            user.Email = email == null ? user.Email : email;
            user.Age = (int)(age == null ? user.Age : age);
            user.Gender = gender == null ? user.Gender : gender;
            user.Adress = adress == null ? user.Adress : adress;
            user.Permission = permission == null ? user.Permission : permission;
            user.TaskLists = user.TaskLists;

            _dbContext.SaveChanges();

            return user;

        }

        public string DeleteUser(Guid id)
        {
            User user = _dbContext.Users.First(u => u.Id == id);

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return "User was deleted";
        }

        //Exempel funktion på hur man kan fixa auth genom bearer token
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

        public async Task<User> AuthenticateUser(string username, string password)
        {
            User user;
            try
            {
                user = _dbContext.Users.SingleOrDefault(x => x.Username == username);

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
    }
}
