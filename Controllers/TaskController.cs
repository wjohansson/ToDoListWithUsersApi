using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet("AllTasks")]
        public IActionResult GetTasks()
        {
            return Ok(_taskService.GetTasks());
        }

        [HttpGet("CurrentListTasks")]
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

        [HttpGet("{taskId}")]
        public IActionResult GetTask(Guid taskId)
        {
            return Ok(_taskService.GetTask(taskId));
        }

        [HttpPost("Create")]
        public IActionResult CreateTask(string title, string description, Priority priority)
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

        [HttpPut("Edit")]
        public IActionResult EditTask(Guid taskId, string? title, string? description, Priority priority)
        {

            return Ok(_taskService.EditTask(taskId, title, description, priority));
        }

        [HttpPut("{taskId}/Edit")]
        public IActionResult EditCurrentTask(string? title, string? description, Priority priority)
        {
            Guid taskId;

            try
            {
                taskId = Guid.Parse(CurrentRecord.Record["TaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a task.");
            }

            return Ok(_taskService.EditTask(taskId, title, description, priority));
        }

        [HttpPut("ToggleCompletion")]
        public IActionResult ToggleCompletion(Guid taskId)
        {

            return Ok(_taskService.ToggleCompletion(taskId));
        }

        [HttpPut("{taskId}/ToggleCompletion")]
        public IActionResult ToggleCurrentCompletion()
        {
            Guid taskId;

            try
            {
                taskId = Guid.Parse(CurrentRecord.Record["TaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a task.");
            }

            return Ok(_taskService.ToggleCompletion(taskId));
        }

        [HttpPut("SortSubTasksBy")]
        public IActionResult UpdateSort(SortSubTasks sortBy)
        {
            Guid taskId;

            try
            {
                taskId = Guid.Parse(CurrentRecord.Record["TaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a task.");
            }

            return Ok(_taskService.UpdateSort(taskId, sortBy));
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteTask(Guid taskId)
        {
            return Ok(_taskService.DeleteTask(taskId));
        }

        [HttpDelete("{taskId}/Delete")]
        public IActionResult DeleteCurrentTask()
        {
            Guid taskId;

            try
            {
                taskId = Guid.Parse(CurrentRecord.Record["TaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a task.");
            }

            return Ok(_taskService.DeleteTask(taskId));
        }
    }
}
