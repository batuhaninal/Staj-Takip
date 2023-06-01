using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class InternshipDocument_StudentUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentUserId",
                table: "InternshipDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipDocuments_StudentUserId",
                table: "InternshipDocuments",
                column: "StudentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipDocuments_StudentUsers_StudentUserId",
                table: "InternshipDocuments",
                column: "StudentUserId",
                principalTable: "StudentUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipDocuments_StudentUsers_StudentUserId",
                table: "InternshipDocuments");

            migrationBuilder.DropIndex(
                name: "IX_InternshipDocuments_StudentUserId",
                table: "InternshipDocuments");

            migrationBuilder.DropColumn(
                name: "StudentUserId",
                table: "InternshipDocuments");
        }
    }
}
