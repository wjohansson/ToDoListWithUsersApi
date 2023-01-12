using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoListWithUsersApi.Services;
using Task = ToDoListWithUsersApi.Models.Task;

namespace ToDoListWithUsersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService service)
        {
            _taskService = service;
        }

        [HttpGet("GetCurrentListTasks")]
        public IActionResult GetCurrentListTasks()
        {
            Guid listId;
            try
            {
                listId = Guid.Parse(CurrentRecord.Record["ListId"]);

            }
            catch (Exception)
            {
                return BadRequest("Not inside of a list.");
            }
            return Ok(_taskService.GetCurrentListTasks(listId));
        }

        //[HttpGet("GetAllTasks")]
        //public IActionResult GetAllTasks()
        //{
        //    return Ok(_taskService.GetAllTasks(listId));
        //}

        [HttpGet("{id}")]
        public IActionResult GetSingleTask(Guid id)
        {
            return Ok(_taskService.GetSingleTask(id));
        }

        [HttpPost("AddTask")]
        public IActionResult AddTask(string title, string description, Priority priority)
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
            return Ok(_taskService.CreateTask(listId, title, description, priority));
        }

        [HttpPut("EditTask")]
        public IActionResult UpdateTask(string? title, string? description, bool? completed, Priority priority)
        {
            Guid id;

            try
            {
                id = Guid.Parse(CurrentRecord.Record["TaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a list.");
            }

            return Ok(_taskService.UpdateTask(id, title, description, completed, priority));
        }

        [HttpDelete("DeleteTask")]
        public IActionResult DeleteTask()
        {
            Guid id;

            try
            {
                id = Guid.Parse(CurrentRecord.Record["TaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a list.");
            }

            return Ok(_taskService.DeleteTask(id));
        }
    }
}
