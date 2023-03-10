using Microsoft.AspNetCore.Mvc;
using ToDoListWithUsersApi.Services;
using Microsoft.AspNetCore.Authorization;
using DataLibrary;
using DataLibrary.Models;

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
        public async Task<ActionResult<List<SubTaskModel>>> GetAllSubTasks()
        {
            return Ok(_taskService.GetAllSubTasks());
        }

        [HttpGet("CurrentTaskSubTasks")]
        public async Task<ActionResult<List<SubTaskModel>>> GetCurrentTaskSubTasks()
        {
            try
            {
                return Ok(_taskService.GetCurrentTaskSubTasks());
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with getting the sub tasks");
            }
        }

        [HttpPut("SubTask")]
        public async Task<ActionResult<SubTaskModel>> GetSubTask()
        {
            try
            {
                Guid subTaskId = Request.ReadFromJsonAsync<Guid>().Result;
                return Ok(_taskService.GetSubTask(subTaskId));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with getting the sub task");
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<SubTaskModel>> CreateSubTask()
        {
            try
            {
                SubTaskModel? subTask = Request.ReadFromJsonAsync<SubTaskModel>().Result;
                return Ok(_taskService.CreateSubTask(subTask));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with creating the sub tasks");
            }
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<SubTaskModel>> EditSubTask()
        {
            try
            {
                SubTaskModel? subTask = Request.ReadFromJsonAsync<SubTaskModel>().Result;
                return Ok(_taskService.EditSubTask(subTask));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with editing the sub task");
            }
        }

        [HttpPut("Delete")]
        public async Task<ActionResult<SubTaskModel>> DeleteCurrentSubTask()
        {
            try
            {
                SubTaskModel? subTask = Request.ReadFromJsonAsync<SubTaskModel>().Result;
                return Ok(_taskService.DeleteSubTask(subTask));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with deleting the sub task");
            }
        }
    }
}
