using DataLibrary;
using DataLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ToDoListWithUsersApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly UserContext _dbContext;

        public CategoryService(UserContext context)
        {
            _dbContext = context;
        }
        public CategoryModel CreateCategory(Guid userId, CategoryModel category)
        {
            if (_dbContext.Categories.Any(x => x.Title == category.Title))
            {
                throw new DuplicateNameException("Category already exists");
            }

            category.UserId = userId;

            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return category;
        }

        public CategoryModel DeleteCategory(CategoryModel category)
        {
            CategoryModel oldCategory = _dbContext.Categories.Include(x => x.TaskLists).First(u => u.Id == category.Id);
            
            if (oldCategory.Title == "No category")
            {
                throw new Exception();
            }

            CategoryModel noCategory = _dbContext.Categories.First(u => u.Title == "No category" && u.UserId == oldCategory.UserId);

            foreach (var list in oldCategory.TaskLists)
            {
                list.CategoryId = noCategory.Id;
            }

            _dbContext.Categories.Remove(oldCategory);
            _dbContext.SaveChanges();

            return oldCategory;
        }

        public List<CategoryModel> GetCategories()
        {
            var categories = _dbContext.Categories.Include(x => x.TaskLists).ToList();

            return categories;
        }

        public List<CategoryModel> GetCurrentUserCategories(Guid userId)
        {
            var categories = GetCategories();
            var currentCategories = categories.Where(x => x.UserId == userId).ToList();

            return currentCategories;
        }

        public CategoryModel GetCategory(CategoryModel category)
        {
            var currentCategory = _dbContext.Categories.Include(x => x.TaskLists).First(x => x.Id == category.Id);
            CurrentActive.Id["CategoryId"] = category.Id;
            return currentCategory;
        }

        public CategoryModel EditCategory(CategoryModel newCategory)
        {
            CategoryModel category = _dbContext.Categories.Include(x => x.TaskLists).First(u => u.Id == newCategory.Id);

            category.Title = newCategory.Title ?? category.Title;

            _dbContext.SaveChanges();

            return category;
        }
    }
}
