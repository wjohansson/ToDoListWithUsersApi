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

        [HttpGet("GetCurrentTaskSubTasks")]
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

        [HttpGet("{id}")]
        public IActionResult GetSingleSubTask(Guid id)
        {
            return Ok(_taskService.GetSingleSubTask(id));
        }

        [HttpPost("AddSubTask")]
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

        [HttpPut("EditSubTask")]
        public IActionResult UpdateSubTask(string? title, string? description)
        {
            Guid id;

            try
            {
                id = Guid.Parse(CurrentRecord.Record["SubTaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a task.");
            }

            return Ok(_taskService.UpdateSubTask(id, title, description));
        }

        [HttpDelete("DeleteSubTask")]
        public IActionResult DeleteSubTask()
        {
            Guid id;

            try
            {
                id = Guid.Parse(CurrentRecord.Record["SubTaskId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not inside of a task.");
            }

            return Ok(_taskService.DeleteSubTask(id));
        }
    }
}
