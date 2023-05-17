using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class booktemplate_current : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BookTemplates",
                newName: "Title");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "BookTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "BookTemplates");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "BookTemplates",
                newName: "Name");
        }
    }
}
