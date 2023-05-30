using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class internshipsbook_entity_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "InternshipsBooks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "InternshipsBooks");

            migrationBuilder.DropColumn(
                name: "WorkDays",
                table: "InternshipsBooks");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompanyChecked",
                table: "InternshipsBooks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeacherChecked",
                table: "InternshipsBooks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StudentUserId",
                table: "InternshipsBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipsBooks_StudentUserId",
                table: "InternshipsBooks",
                column: "StudentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipsBooks_StudentUsers_StudentUserId",
                table: "InternshipsBooks",
                column: "StudentUserId",
                principalTable: "StudentUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipsBooks_StudentUsers_StudentUserId",
                table: "InternshipsBooks");

            migrationBuilder.DropIndex(
                name: "IX_InternshipsBooks_StudentUserId",
                table: "InternshipsBooks");

            migrationBuilder.DropColumn(
                name: "IsCompanyChecked",
                table: "InternshipsBooks");

            migrationBuilder.DropColumn(
                name: "IsTeacherChecked",
                table: "InternshipsBooks");

            migrationBuilder.DropColumn(
                name: "StudentUserId",
                table: "InternshipsBooks");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "InternshipsBooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "InternshipsBooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkDays",
                table: "InternshipsBooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
