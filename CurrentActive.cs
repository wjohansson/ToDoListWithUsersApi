namespace ToDoListWithUsersApi
{
    public class CurrentActive
    {
        public static Dictionary<string, Guid> Id = new()
        {
            { "UserId", Guid.Empty },
            { "CategoryId", Guid.Empty },
            { "ListId", Guid.Empty },
            { "TaskId", Guid.Empty },
            { "SubTaskId", Guid.Empty }
        };
    }
}
