using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListWithUsersApi.Migrations
{
    public partial class ChangedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryTitle",
                table: "Categories",
                newName: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Categories",
                newName: "CategoryTitle");
        }
    }
}
