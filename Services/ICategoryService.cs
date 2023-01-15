namespace ToDoListWithUsersApi.Services
{
    public interface ICategoryService
    {
        List<Category> GetCategories();

        List<Category> GetCurrentUserCategories(Guid userId);

        Category GetCategory(Guid categoryId);

        Category CreateCategory(Guid userId, string title);

        Category EditCategory(Guid categoryId, string? title);

        string DeleteCategory(Guid categoryId);
    }
}
