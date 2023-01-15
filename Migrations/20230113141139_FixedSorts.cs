using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListWithUsersApi.Migrations
{
    public partial class FixedSorts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SortBy",
                table: "Users",
                newName: "SortLists");

            migrationBuilder.RenameColumn(
                name: "SortBy",
                table: "Tasks",
                newName: "SortSubTasks");

            migrationBuilder.RenameColumn(
                name: "SortBy",
                table: "TaskLists",
                newName: "SortTasks");

            migrationBuilder.RenameColumn(
                name: "SortBy",
                table: "Categories",
                newName: "SortLists");

            migrationBuilder.AddColumn<int>(
                name: "SortCategories",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortCategories",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SortLists",
                table: "Users",
                newName: "SortBy");

            migrationBuilder.RenameColumn(
                name: "SortSubTasks",
                table: "Tasks",
                newName: "SortBy");

            migrationBuilder.RenameColumn(
                name: "SortTasks",
                table: "TaskLists",
                newName: "SortBy");

            migrationBuilder.RenameColumn(
                name: "SortLists",
                table: "Categories",
                newName: "SortBy");
        }
    }
}
