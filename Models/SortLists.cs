using System.Text.Json.Serialization;

namespace ToDoListWithUsersApi.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortLists
    {
        Name,
        New,
        Old,
        Category
    }
}
