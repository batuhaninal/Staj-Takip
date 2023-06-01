using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class update_signature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signatures_InternshipsBooks_InternshipsBookId",
                table: "Signatures");

            migrationBuilder.DropIndex(
                name: "IX_Signatures_InternshipsBookId",
                table: "Signatures");

            migrationBuilder.DropColumn(
                name: "InternshipBookId",
                table: "Signatures");

            migrationBuilder.DropColumn(
                name: "InternshipsBookId",
                table: "Signatures");

            migrationBuilder.DropColumn(
                name: "IsLecturer",
                table: "Signatures");

            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "Signatures");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Signatures",
                newName: "Name");

            migrationBuilder.AddColumn<byte[]>(
                name: "SignatureData",
                table: "Signatures",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureData",
                table: "Signatures");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Signatures",
                newName: "Path");

            migrationBuilder.AddColumn<int>(
                name: "InternshipBookId",
                table: "Signatures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternshipsBookId",
                table: "Signatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLecturer",
                table: "Signatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "Signatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Signatures_InternshipsBookId",
                table: "Signatures",
                column: "InternshipsBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Signatures_InternshipsBooks_InternshipsBookId",
                table: "Signatures",
                column: "InternshipsBookId",
                principalTable: "InternshipsBooks",
                principalColumn: "Id");
        }
    }
}
