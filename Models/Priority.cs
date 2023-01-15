using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ToDoListWithUsersApi.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Priority
    {
        Low,
        MediumLow,
        Medium,
        MediumHigh,
        High
    }
}
