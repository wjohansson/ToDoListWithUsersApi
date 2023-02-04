using DataLibrary;
using DataLibrary.Enums;
using DataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;
using ToDoListWithUsersApi.Services;

namespace ToDoListWithUsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }

        [Authorize(Roles = "System,Admin,Moderator")]
        [HttpGet("AllUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [Authorize]
        [HttpGet("User")]
        public IActionResult GetUser()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                Guid userId = Guid.Empty;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    userId = Guid.Parse(claims.First(x => x.Type == "UserId").Value);
                }

                return Ok(_userService.GetUser(userId));
            }
            catch (Exception)
            {
                return BadRequest("Could not find user");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login()
        {
            UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;

            try
            {
                return Ok(_userService.Login(user));
            }
            catch (Exception e) when (e.InnerException is InvalidOperationException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception e) when (e.InnerException is UnauthorizedAccessException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with the login");
            }
        }

        [Authorize]
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            return Ok(_userService.Logout());
        }

        [Authorize]
        [AllowAnonymous]
        [HttpPost("Create")]
        public IActionResult CreateUser()
        {
            try
            {
                UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;
                var newUser = _userService.CreateUser(user);

                _userService.Login(user);
                return Ok(newUser);
            }
            catch (Exception e) when (e.InnerException is DuplicateNameException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with creating the user");
            }

        }

        [Authorize]
        [HttpPut("Edit")]
        public IActionResult EditUser()
        {
            try
            {
                UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;
                return Ok(_userService.EditUser(user));
            }
            catch (Exception e) when (e.InnerException is DuplicateNameException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception e) when (e.InnerException is InvalidOperationException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with editing the user");
            }
        }

        [Authorize(Roles = "System, Admin, Moderator")]
        [HttpPut("Promote")]
        public IActionResult PromoteAnotherUser()
        {
            try
            {
                UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;
                return Ok(_userService.PromoteUser(user));
            }
            catch (Exception e) when (e.InnerException is InvalidOperationException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with promoting the user");
            }
        }

        [Authorize(Roles = "System, Admin, Moderator")]
        [HttpPut("Demote")]
        public IActionResult DemoteAnotherUser()
        {
            try
            {
                UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;
                return Ok(_userService.DemoteUser(user));
            }
            catch (Exception e) when (e.InnerException is InvalidOperationException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with demoting the user");
            }
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword()
        {
            try
            {
                UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;
                return Ok(_userService.ChangePassword(user));
            }
            catch (Exception e) when (e.InnerException is InvalidOperationException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception e) when (e.InnerException is UnauthorizedAccessException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with changing your password");
            }
        }

        [Authorize(Roles = "System, Admin, Moderator")]
        [HttpPut("ChangeOtherPassword")]
        public IActionResult ChangeOtherPassword()
        {
            try
            {
                UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;
                return Ok(_userService.ChangeOtherPassword(user));
            }
            catch (Exception e) when (e.InnerException is InvalidOperationException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with changing the users password");
            }
        }

        [Authorize]
        [HttpPut("SortListsBy")]
        public IActionResult UpdateSort()
        {
            try
            {
                UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;
                return Ok(_userService.UpdateSort(user));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with the sort update");
            }
        }

        [Authorize(Roles = "System, Admin, Moderator")]
        [HttpPut("DeleteAnother")]
        public IActionResult DeleteAnotherUser()
        {
            try
            {
                UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;
                return Ok(_userService.DeleteUser(user));
            }
            catch (Exception e) when (e.InnerException is InvalidOperationException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with deleting the user");
            }
            
        }

        [Authorize]
        [HttpPut("Delete")]
        public IActionResult DeleteUser()
        {
            try
            {
                UserModel? user = Request.ReadFromJsonAsync<UserModel>().Result;
                var userToDelete = _userService.DeleteUser(user);
                _userService.Logout();
                return Ok(userToDelete);
            }
            catch (Exception e) when (e.InnerException is InvalidOperationException)
            {
                return BadRequest(e.InnerException.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with deleting the user");
            }
        }
    }
}
