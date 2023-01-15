using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using ToDoListWithUsersApi.Services;

namespace ToDoListWithUsersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }

        [HttpGet("AllUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("CurrentUser")]
        public IActionResult GetUser()
        {
            Guid userId;

            try
            {
                userId = Guid.Parse(CurrentRecord.Record["UserId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in");
            }

            return Ok(_userService.GetUser(userId));
        }

        [HttpGet("{userId}")]
        public IActionResult GetAnotherUser(Guid userId)
        {
            return Ok(_userService.GetUser(userId));
        }

        // https://www.infoworld.com/article/3650668/implement-authorization-for-swagger-in-aspnet-core-6.html
        //Exempel funktion på hur login funktion kan se ut med bearer token
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(string? username, string? password)
        {
            try
            {
                return Ok(_userService.Login(username, password).Result);
            }
            catch (Exception e) when (e.InnerException is InvalidOperationException)
            {
                return BadRequest("Username and Password is required");
            }
            catch (Exception e) when (e.InnerException is UnauthorizedAccessException)
            {
                return BadRequest("Invalid login");
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with creating the token");
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            return Ok(_userService.Logout());
        }

        [HttpPost("CreateAnotherUser")]
        public IActionResult CreateAnotherUser(string username, string password, string firstName, string lastName, string email, int age, string gender, string adress, PermissionLevel permission)
        {
            return Ok(_userService.CreateUser(username, password, firstName, lastName, email, age, gender, adress, permission));
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public IActionResult CreateUser(string username, string password, string firstName, string lastName, string email, int age, string gender, string adress)
        {
            User user = _userService.CreateUser(username, password, firstName, lastName, email, age, gender, adress, null);
            _userService.Login(username, password);
            return Ok(user);
        }

        [HttpPut("Edit")]
        public IActionResult UpdateAnotherUser(Guid userId, string? username, string? password, string? firstName, string? lastName, string? email, int? age, string? gender, string? adress, PermissionLevel permission)
        {
            return Ok(_userService.EditUser(userId, username, password, firstName, lastName, email, age, gender, adress, permission));
        }

        [HttpPut("{userId}/Edit")]
        public IActionResult EditUser(string? username, string? password, string? firstName, string? lastName, string? email, int? age, string? gender, string? adress)
        {
            Guid userId;

            try
            {
                userId = Guid.Parse(CurrentRecord.Record["UserId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in.");
            }

            return Ok(_userService.EditUser(userId, username, password, firstName, lastName, email, age, gender, adress, null));
        }

        [HttpPut("Promote")]
        public IActionResult PromoteAnotherUser(Guid userId)
        {
            User user;

            try
            {
                user = _userService.PromoteUser(userId);
            }
            catch (Exception e) when (e.InnerException == new InvalidOperationException())
            {
                return BadRequest(e.Message);
            }

            return Ok(user);
        }

        [HttpPut("Demote")]
        public IActionResult DemoteAnotherUser(Guid userId)
        {
            User user;

            try
            {
                user = _userService.DemoteUser(userId);
            }
            catch (Exception e) when (e.InnerException == new InvalidOperationException())
            {
                return BadRequest(e.Message);
            }

            return Ok(user);
        }

        [HttpPut("SortListsBy")]
        public IActionResult UpdateSort(SortLists sortBy)
        {
            Guid userId;

            try
            {
                userId = Guid.Parse(CurrentRecord.Record["UserId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in.");
            }

            return Ok(_userService.UpdateSort(userId, sortBy));
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteAnotherUser(Guid userId)
        {
            return Ok(_userService.DeleteUser(userId));
        }

        [HttpDelete("{userId}/Delete")]
        public IActionResult DeleteUser()
        {
            Guid userId;

            try
            {
                userId = Guid.Parse(CurrentRecord.Record["UserId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in.");
            }

            return Ok(new List<string> { _userService.DeleteUser(userId), _userService.Logout() });
        }
    }
}
