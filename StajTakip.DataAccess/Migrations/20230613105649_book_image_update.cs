using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class book_image_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookImages_InternshipsBooks_InternshipsBookId",
                table: "BookImages");

            migrationBuilder.DropIndex(
                name: "IX_BookImages_InternshipsBookId",
                table: "BookImages");

            migrationBuilder.DropColumn(
                name: "InternshipBookId",
                table: "BookImages");

            migrationBuilder.DropColumn(
                name: "InternshipsBookId",
                table: "BookImages");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "BookImages",
                newName: "ImageName");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "BookImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "BookImages");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "BookImages",
                newName: "Path");

            migrationBuilder.AddColumn<int>(
                name: "InternshipBookId",
                table: "BookImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternshipsBookId",
                table: "BookImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookImages_InternshipsBookId",
                table: "BookImages",
                column: "InternshipsBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookImages_InternshipsBooks_InternshipsBookId",
                table: "BookImages",
                column: "InternshipsBookId",
                principalTable: "InternshipsBooks",
                principalColumn: "Id");
        }
    }
}
