using Microsoft.AspNetCore.Mvc;
using ToDoListWithUsersApi.Services;
using Microsoft.AspNetCore.Authorization;
using DataLibrary;
using DataLibrary.Models;
using System.Security.Claims;

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
        public async Task<ActionResult<List<CategoryModel>>> GetCategories()
        {
            return Ok(_categoryService.GetCategories());
        }

        [HttpGet("CurrentUserCategories")]
        public async Task<ActionResult<List<CategoryModel>>> GetCurrentUserCategories()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                Guid userId = Guid.Empty;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    userId = Guid.Parse(claims.First(x => x.Type == "UserId").Value);
                }

                return Ok(_categoryService.GetCurrentUserCategories(userId));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with getting the categories");
            }
        }

        [HttpPut("Category")]
        public async Task<ActionResult<CategoryModel>> GetCategory()
        {
            try
            {
                CategoryModel? category = Request.ReadFromJsonAsync<CategoryModel>().Result;

                return Ok(_categoryService.GetCategory(category));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with getting the category");
            }
            }

        [HttpPost("Create")]
        public async Task<ActionResult<CategoryModel>> CreateCategory()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                Guid userId = Guid.Empty;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    userId = Guid.Parse(claims.First(x => x.Type == "UserId").Value);
                }

                CategoryModel? category = Request.ReadFromJsonAsync<CategoryModel>().Result;
                return Ok(_categoryService.CreateCategory(userId, category));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with creating the category");
            }
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<CategoryModel>> EditCurrentCategory()
        {
            try
            {
                CategoryModel? category = Request.ReadFromJsonAsync<CategoryModel>().Result;
                return Ok(_categoryService.EditCategory(category));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with editing the category");
            }
        }

        [HttpPut("Delete")]
        public async Task<ActionResult<CategoryModel>> DeleteCurrentCategory()
        {
            try
            {
                CategoryModel? category = Request.ReadFromJsonAsync<CategoryModel>().Result;
                return Ok(_categoryService.DeleteCategory(category));
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong with deleting the category");
            }
        }
    }
}
