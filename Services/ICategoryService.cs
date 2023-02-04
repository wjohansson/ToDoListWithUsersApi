using DataLibrary.Models;

namespace ToDoListWithUsersApi.Services
{
    public interface ICategoryService
    {
        List<CategoryModel> GetCategories();

        List<CategoryModel> GetCurrentUserCategories(Guid userId);

        CategoryModel GetCategory(CategoryModel category);

        CategoryModel CreateCategory(Guid userId, CategoryModel category);

        CategoryModel EditCategory(CategoryModel newCategory);

        CategoryModel DeleteCategory(CategoryModel category);
    }
}
