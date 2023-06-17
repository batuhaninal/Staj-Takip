using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class message_read_operations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompanyRead",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStudentRead",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeacherRead",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompanyRead",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsStudentRead",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsTeacherRead",
                table: "Messages");
        }
    }
}
