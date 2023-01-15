using System.Text.Json.Serialization;

namespace ToDoListWithUsersApi.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortSubTasks
    {
        Name,
        New,
        Old
    }
}
