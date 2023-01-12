using System.ComponentModel;

namespace ToDoListWithUsersApi.Models
{
    public enum PermissionLevel
    {
        [Description("User")]
        User,
        [Description("Moderator")]
        Moderator,
        [Description("Admin")]
        Admin,
        [Description("System")]
        System
    }
}
