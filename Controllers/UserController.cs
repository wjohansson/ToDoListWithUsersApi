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

        [HttpGet("GetAllUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("GetCurrentUser")]
        public IActionResult GetUser()
        {
            Guid id;

            try
            {
                //id = Guid.Parse(CurrentRecord.Record["UserId"]);
                id = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "UserId").Value);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in");
            }

            return Ok(_userService.GetSingleUser(id));
        }

        [HttpGet("{id}")]
        public IActionResult GetAnotherUser(Guid id)
        {
            return Ok(_userService.GetSingleUser(id));
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

        [HttpPost("AddAnotherUser")]
        public IActionResult AddAnotherUser(string username, string password, string firstName, string lastName, string email, int age, string gender, string adress, PermissionLevel permission)
        {
            return Ok(_userService.CreateUser(username, password, firstName, lastName, email, age, gender, adress, permission));
        }

        [AllowAnonymous]
        [HttpPost("AddUser")]
        public IActionResult AddUser(string username, string password, string firstName, string lastName, string email, int age, string gender, string adress, PermissionLevel permission)
        {
            return Ok(_userService.CreateUser(username, password, firstName, lastName, email, age, gender, adress, permission));
        }

        [HttpPut("EditAnotherUser")]
        public IActionResult UpdateAnotherUser(Guid id, string? username, string? password, string? firstName, string? lastName, string? email, int? age, string? gender, string? adress, PermissionLevel permission)
        {
            return Ok(_userService.UpdateUser(id, username, password, firstName, lastName, email, age, gender, adress, permission));
        }

        [HttpPut("EditUser")]
        public IActionResult UpdateUser(string? username, string? password, string? firstName, string? lastName, string? email, int? age, string? gender, string? adress, PermissionLevel permission)
        {
            Guid id;

            try
            {
                id = Guid.Parse(CurrentRecord.Record["UserId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in.");
            }

            return Ok(_userService.UpdateUser(id, username, password, firstName, lastName, email, age, gender, adress, permission));
        }

        [HttpDelete("DeleteAnotherUser")]
        public IActionResult DeleteAnotherUser(Guid id)
        {
            return Ok(_userService.DeleteUser(id));
        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser()
        {
            Guid id;

            try
            {
                id = Guid.Parse(CurrentRecord.Record["UserId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in.");
            }

            return Ok(_userService.DeleteUser(id));
        }
    }
}
