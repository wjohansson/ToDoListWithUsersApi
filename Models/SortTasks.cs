using System.Text.Json.Serialization;

namespace ToDoListWithUsersApi.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortTasks
    {
        Name,
        New,
        Old,
        Priority,
        Completion
    }
}
