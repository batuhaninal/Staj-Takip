using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class update_rolename_mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "admin");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 7, 31, 22, 11, 18, 114, DateTimeKind.Local).AddTicks(1854), new DateTime(2024, 7, 31, 22, 11, 18, 114, DateTimeKind.Local).AddTicks(1857), new byte[] { 52, 196, 242, 7, 74, 61, 240, 230, 14, 152, 199, 210, 174, 255, 105, 60, 243, 134, 122, 170, 15, 3, 52, 129, 106, 202, 185, 44, 65, 161, 205, 24 }, new byte[] { 240, 233, 87, 126, 155, 92, 97, 89, 0, 105, 224, 81, 84, 183, 55, 191, 130, 96, 33, 187, 143, 76, 69, 110, 128, 240, 43, 50, 50, 241, 84, 54, 211, 96, 203, 158, 231, 254, 5, 158, 129, 7, 143, 216, 184, 147, 46, 44, 233, 114, 196, 126, 125, 164, 68, 118, 190, 210, 138, 103, 139, 229, 70, 193 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 7, 31, 22, 6, 58, 891, DateTimeKind.Local).AddTicks(9880), new DateTime(2024, 7, 31, 22, 6, 58, 891, DateTimeKind.Local).AddTicks(9881), new byte[] { 59, 145, 160, 153, 215, 50, 194, 69, 163, 130, 252, 245, 156, 23, 103, 58, 50, 189, 163, 75, 201, 165, 70, 147, 35, 230, 134, 41, 195, 249, 121, 240 }, new byte[] { 7, 158, 242, 74, 12, 7, 222, 2, 164, 58, 54, 29, 168, 196, 51, 174, 158, 164, 148, 136, 92, 118, 221, 11, 217, 255, 151, 73, 191, 123, 240, 85, 54, 38, 120, 140, 16, 29, 44, 106, 76, 224, 25, 25, 193, 237, 158, 155, 207, 61, 133, 98, 152, 93, 248, 39, 2, 203, 27, 133, 146, 172, 234, 45 } });
        }
    }
}
