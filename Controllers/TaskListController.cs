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

        [HttpGet("AllLists")]
        public IActionResult GetLists()
        {
            return Ok(_taskListService.GetAllLists());
        }

        [HttpGet("YourLists")]
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

        [HttpGet("ThisCategoryLists")]
        public IActionResult GetCurrentCategoryLists()
        {
            Guid categoryId;

            try
            {
                categoryId = Guid.Parse(CurrentRecord.Record["CategoryId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a category.");
            }

            return Ok(_taskListService.GetCurrentCategoryLists(categoryId));
        }

        [HttpGet("{listId}")]
        public IActionResult GetList(Guid listId)
        {
            return Ok(_taskListService.GetList(listId));
        }

        [HttpPost("Create")]
        public IActionResult CreateList(string title, Guid? categoryId)
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

        [HttpPut("Edit")]
        public IActionResult EditList(Guid listId, string? title, Guid? categoryId)
        {
            return Ok(_taskListService.EditList(listId, title, categoryId));
        }

        [HttpPut("{listId}/Edit")]
        public IActionResult EditCurrentList(string? title, Guid? categoryId)
        {
            Guid listId;

            try
            {
                listId = Guid.Parse(CurrentRecord.Record["ListId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a list.");
            }

            return Ok(_taskListService.EditList(listId, title, categoryId));
        }

        [HttpPut("SortTasksBy")]
        public IActionResult UpdateSort(SortTasks sortBy)
        {
            Guid listId;

            try
            {
                listId = Guid.Parse(CurrentRecord.Record["ListId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a list.");
            }

            return Ok(_taskListService.UpdateSort(listId, sortBy));
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteList(Guid listId)
        {
            return Ok(_taskListService.DeleteList(listId));
        }


        [HttpDelete("{listId}/Delete")]
        public IActionResult DeleteCurrentList()
        {
            Guid listId;

            try
            {
                listId = Guid.Parse(CurrentRecord.Record["ListId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in.");
            }

            return Ok(_taskListService.DeleteList(listId));
        }
    }
}
