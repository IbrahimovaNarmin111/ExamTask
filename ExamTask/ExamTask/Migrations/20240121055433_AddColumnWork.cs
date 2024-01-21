using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamTask.Migrations
{
    public partial class AddColumnWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Work",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Work",
                table: "Teams");
        }
    }
}
