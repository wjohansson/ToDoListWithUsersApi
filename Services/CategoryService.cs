using ToDoListWithUsersApi.Models;

namespace ToDoListWithUsersApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly UserContext _dbContext;

        public CategoryService(UserContext context)
        {
            _dbContext = context;
        }
        public Category CreateCategory(Guid userId, string title)
        {
            Category category = new()
            {
                Title = title,
                UserId = userId
            };

            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return category;
        }

        public string DeleteCategory(Guid categoryId)
        {
            Category category = _dbContext.Categories.First(u => u.Id == categoryId);

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return "Category was deleted";
        }

        public List<Category> GetCategories()
        {
            return _dbContext.Categories.ToList();
        }

        public List<Category> GetCurrentUserCategories(Guid userId)
        {
            var categories = GetCategories();
            return categories.Where(x => x.UserId == userId).ToList();
        }

        public Category GetCategory(Guid categoryId)
        {
            CurrentRecord.Record["CategoryId"] = categoryId.ToString();
            return _dbContext.Categories.First(x => x.Id == categoryId);
        }

        public Category EditCategory(Guid categoryId, string? title)
        {
            Category category = _dbContext.Categories.First(u => u.Id == categoryId);

            category.Title = title ?? category.Title;

            _dbContext.SaveChanges();

            return category;
        }
    }
}
