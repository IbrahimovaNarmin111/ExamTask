using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamTask.Migrations
{
    public partial class ColumnChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Teams",
                newName: "FullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Teams",
                newName: "Title");
        }
    }
}
