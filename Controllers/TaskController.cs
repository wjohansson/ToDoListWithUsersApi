using DataLibrary;
using DataLibrary.Enums;
using DataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListWithUsersApi.Services;

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
            try
            {
                return Ok(_taskService.GetCurrentListTasks());

            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with getting the tasks");
            }
        }

        [HttpPut("Task")]
        public IActionResult GetTask()
        {

            try
            {
                Guid taskId = Request.ReadFromJsonAsync<Guid>().Result;
                return Ok(_taskService.GetTask(taskId));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with getting the task");
            }

        }

        [HttpPost("Create")]
        public IActionResult CreateTask()
        {
            try
            {
                TaskModel? task = Request.ReadFromJsonAsync<TaskModel>().Result;
                Guid listId;
                listId = CurrentActive.Id["ListId"];
                return Ok(_taskService.CreateTask(listId, task));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with creating the task");
            }
        }

        [HttpPut("Edit")]
        public IActionResult EditCurrentTask()
        {
            try
            {
                TaskModel? task = Request.ReadFromJsonAsync<TaskModel>().Result;
                return Ok(_taskService.EditTask(task));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with editing the task");
            }
        }

        [HttpPut("ToggleCompletion")]
        public IActionResult ToggleCurrentCompletion()
        {
            try
            {
                TaskModel? task = Request.ReadFromJsonAsync<TaskModel>().Result;
                return Ok(_taskService.ToggleCompletion(task));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with updating the completion of the task");
            }

        }

        [HttpPut("SortSubTasksBy")]
        public IActionResult UpdateSort()
        {


            try
            {
                TaskModel? task = Request.ReadFromJsonAsync<TaskModel>().Result;
                return Ok(_taskService.UpdateSort(task));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with updating the sort of the task");
            }

        }

        [HttpPut("Delete")]
        public IActionResult DeleteCurrentTask()
        {
            try
            {
                TaskModel? task = Request.ReadFromJsonAsync<TaskModel>().Result;
                return Ok(_taskService.DeleteTask(task));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with deleting the task");
            }
        }
    }
}
