using System.ComponentModel;

namespace ToDoListWithUsersApi.Models
{
    public enum SortBy
    {
        [Description("Name")]
        Name,
        [Description("New")]
        New,
        [Description("Old")]
        Old,
        [Description("Completion")]
        Completion,
        [Description("Priority")]
        Priority
    }
}
