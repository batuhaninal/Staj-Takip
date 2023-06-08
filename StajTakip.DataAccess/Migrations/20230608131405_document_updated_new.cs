using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class document_updated_new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "InternshipDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompanyChecked",
                table: "InternshipDocuments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeacherChecked",
                table: "InternshipDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "InternshipDocuments");

            migrationBuilder.DropColumn(
                name: "IsCompanyChecked",
                table: "InternshipDocuments");

            migrationBuilder.DropColumn(
                name: "IsTeacherChecked",
                table: "InternshipDocuments");
        }
    }
}
