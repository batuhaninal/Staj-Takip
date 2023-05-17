using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class template_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BookTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "BookTemplates");
        }
    }
}
