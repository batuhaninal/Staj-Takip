using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class bookimage_internshipbook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InternshipsBookId",
                table: "BookImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookImages_InternshipsBookId",
                table: "BookImages",
                column: "InternshipsBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookImages_InternshipsBooks_InternshipsBookId",
                table: "BookImages",
                column: "InternshipsBookId",
                principalTable: "InternshipsBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookImages_InternshipsBooks_InternshipsBookId",
                table: "BookImages");

            migrationBuilder.DropIndex(
                name: "IX_BookImages_InternshipsBookId",
                table: "BookImages");

            migrationBuilder.DropColumn(
                name: "InternshipsBookId",
                table: "BookImages");
        }
    }
}
