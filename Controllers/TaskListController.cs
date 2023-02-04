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
            try
            {
            return Ok(_taskListService.GetCurrentUserLists());
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with getting the lists");
            }
        }

        [HttpGet("ThisCategoryLists")]
        public IActionResult GetCurrentCategoryLists()
        {
            try
            {
                return Ok(_taskListService.GetCurrentCategoryLists());
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with getting the lists");
            }
        }

        [HttpPut("List")]
        public IActionResult GetList()
        {
            try
            {
                Guid listId = Request.ReadFromJsonAsync<Guid>().Result;
                return Ok(_taskListService.GetList(listId));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with getting the list");
            }

        }

        [HttpPost("Create")]
        public IActionResult CreateList()
        {
            try
            {
                Guid userId = CurrentActive.Id["UserId"];
                TaskListModel? list = Request.ReadFromJsonAsync<TaskListModel>().Result;
                return Ok(_taskListService.CreateList(userId, list));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with creating the list");
            }
        }

        [HttpPut("Edit")]
        public IActionResult EditCurrentList()
        {
            try
            {
                TaskListModel? list = Request.ReadFromJsonAsync<TaskListModel>().Result;
                return Ok(_taskListService.EditList(list));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with editing the list");
            }
        }

        [HttpPut("SortTasksBy")]
        public IActionResult UpdateSort()
        {
            try
            {
                TaskListModel? list = Request.ReadFromJsonAsync<TaskListModel>().Result;
                return Ok(_taskListService.UpdateSort(list));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with updating the sort of the list");
            }

        }

        [HttpPut("Delete")]
        public IActionResult DeleteCurrentList()
        {
            try
            {
                TaskListModel? list = Request.ReadFromJsonAsync<TaskListModel>().Result;
                return Ok(_taskListService.DeleteList(list));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with deleting the list");
            }
        }
    }
}
