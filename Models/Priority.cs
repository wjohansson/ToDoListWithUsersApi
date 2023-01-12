using System.ComponentModel;

namespace ToDoListWithUsersApi.Models
{
    public enum Priority
    {
        [Description("Low")]
        Low = 1,
        [Description("Medium Low")]
        MediumLow,
        [Description("Medium")]
        Medium,
        [Description("Medium High")]
        MediumHigh,
        [Description("High")]
        High
    }
}
