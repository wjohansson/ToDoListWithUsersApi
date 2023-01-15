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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService service)
        {
            _categoryService = service;
        }

        [HttpGet("AllCategories")]
        public IActionResult GetCategories()
        {
            return Ok(_categoryService.GetCategories());
        }

        [HttpGet("CurrentUserCategories")]
        public IActionResult GetCurrentUserCategories()
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

            return Ok(_categoryService.GetCurrentUserCategories(userId));
        }

        [HttpGet("{categoryId}")]
        public IActionResult GetCategory(Guid categoryId)
        {
            return Ok(_categoryService.GetCategory(categoryId));
        }

        [HttpPost("Create")]
        public IActionResult CreateCategory(string title)
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

            return Ok(_categoryService.CreateCategory(userId, title));
        }

        [HttpPut("Edit")]
        public IActionResult EditCategory(Guid categoryId, string? title)
        {
            return Ok(_categoryService.EditCategory(categoryId, title));
        }

        [HttpPut("{categoryId}/Edit")]
        public IActionResult EditCurrentCategory(string? title)
        {
            Guid categoryId;

            try
            {
                categoryId = Guid.Parse(CurrentRecord.Record["CategoryId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in.");
            }

            return Ok(_categoryService.EditCategory(categoryId, title));
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteCategory(Guid categoryId)
        {
            return Ok(_categoryService.DeleteCategory(categoryId));
        }

        [HttpDelete("{categoryId}/Delete")]
        public IActionResult DeleteCurrentCategory()
        {
            Guid categoryId;

            try
            {
                categoryId = Guid.Parse(CurrentRecord.Record["CategoryId"]);
            }
            catch (FormatException)
            {
                return BadRequest("Not logged in.");
            }

            return Ok(_categoryService.DeleteCategory(categoryId));
        }
    }
}
