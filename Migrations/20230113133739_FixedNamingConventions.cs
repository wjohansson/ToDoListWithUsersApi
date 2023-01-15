using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListWithUsersApi.Migrations
{
    public partial class FixedNamingConventions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Users_UserId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLists_Category_Categoryid",
                table: "TaskLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameColumn(
                name: "Categoryid",
                table: "TaskLists",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLists_Categoryid",
                table: "TaskLists",
                newName: "IX_TaskLists_CategoryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Category_UserId",
                table: "Categories",
                newName: "IX_Categories_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLists_Categories_CategoryId",
                table: "TaskLists",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLists_Categories_CategoryId",
                table: "TaskLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "TaskLists",
                newName: "Categoryid");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLists_CategoryId",
                table: "TaskLists",
                newName: "IX_TaskLists_Categoryid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Category",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_UserId",
                table: "Category",
                newName: "IX_Category_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Users_UserId",
                table: "Category",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLists_Category_Categoryid",
                table: "TaskLists",
                column: "Categoryid",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
