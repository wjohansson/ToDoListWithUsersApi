using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoListWithUsersApi.Models;
using ToDoListWithUsersApi.Services;

namespace ToDoListWithUsersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskListController : ControllerBase
    {
        private readonly ITaskListService _taskListService;

        public TaskListController(ITaskListService service)
        {
            _taskListService = service;
        }

        [HttpGet("GetAllLists")]
        public IActionResult GetLists()
        {
            return Ok(_taskListService.GetAllLists());
        }

        [HttpGet("GetCurrentUserLists")]
        public IActionResult GetCurrentUserLists()
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

            return Ok(_taskListService.GetCurrentUserLists(userId));
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleList(Guid id)
        {
            return Ok(_taskListService.GetSingleList(id));
        }

        [HttpPost("AddList")]
        public IActionResult AddList(string title, Guid categoryId)
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

            return Ok(_taskListService.CreateList(userId, title, categoryId));
        }

        [HttpPut("EditList")]
        public IActionResult UpdateList(Guid id, string? title, Guid categoryId)
        {
            return Ok(_taskListService.UpdateList(id, title, categoryId));
        }

        [HttpDelete("DeleteList")]
        public IActionResult DeleteList(Guid id)
        {
            return Ok(_taskListService.DeleteList(id));
        }
    }
}
