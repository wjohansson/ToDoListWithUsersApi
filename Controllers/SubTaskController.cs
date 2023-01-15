using Microsoft.AspNetCore.Mvc;
using ToDoListWithUsersApi;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoListWithUsersApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace ToDoListWithUsersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubTaskController : ControllerBase
    {
        private readonly ISubTaskService _taskService;

        public SubTaskController(ISubTaskService service)
        {
            _taskService = service;
        }

        [HttpGet("AllSubTasks")]
        public IActionResult GetAllSubTasks()
        {
            return Ok(_taskService.GetAllSubTasks());
        }

        [HttpGet("CurrentTaskSubTasks")]
        public IActionResult GetCurrentTaskSubTasks()
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

            return Ok(_taskService.GetCurrentTaskSubTasks(taskId));
        }

        [HttpGet("{subTaskId}")]
        public IActionResult GetSingleSubTask(Guid subTaskId)
        {
            return Ok(_taskService.GetSingleSubTask(subTaskId));
        }

        [HttpPost("Add")]
        public IActionResult AddSubTask(string title, string description)
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

            return Ok(_taskService.CreateSubTask(taskId, title, description));
        }

        [HttpPut("Edit")]
        public IActionResult UpdateSubTask(Guid subTaskId, string? title, string? description)
        {
            return Ok(_taskService.UpdateSubTask(subTaskId, title, description));
        }

        [HttpPut("{subTaskId}/Edit")]
        public IActionResult UpdateCurrentSubTask(string? title, string? description)
        {
            Guid subTaskId;

            try
            {
                subTaskId = Guid.Parse(CurrentRecord.Record["SubTaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a sub task.");
            }

            return Ok(_taskService.UpdateSubTask(subTaskId, title, description));
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteSubTask(Guid subTaskId)
        {

            return Ok(_taskService.DeleteSubTask(subTaskId));
        }

        [HttpDelete("{subTaskId}/Delete")]
        public IActionResult DeleteCurrentSubTask()
        {
            Guid subTaskId;

            try
            {
                subTaskId = Guid.Parse(CurrentRecord.Record["SubTaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a sub task.");
            }

            return Ok(_taskService.DeleteSubTask(subTaskId));
        }
    }
}
