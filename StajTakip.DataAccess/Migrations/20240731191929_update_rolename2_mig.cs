using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class update_rolename2_mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OperationClaims",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "admin.teacher" },
                    { 3, "admin.company" },
                    { 4, "student" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 7, 31, 22, 19, 29, 366, DateTimeKind.Local).AddTicks(7284), new DateTime(2024, 7, 31, 22, 19, 29, 366, DateTimeKind.Local).AddTicks(7285), new byte[] { 187, 3, 211, 40, 178, 50, 2, 94, 206, 88, 249, 56, 8, 156, 55, 72, 96, 102, 233, 211, 37, 253, 222, 164, 238, 176, 223, 233, 138, 56, 50, 234 }, new byte[] { 3, 113, 115, 144, 36, 250, 237, 243, 226, 200, 255, 162, 254, 231, 38, 237, 65, 186, 154, 240, 220, 92, 232, 190, 186, 7, 220, 180, 200, 138, 131, 70, 143, 97, 156, 55, 58, 73, 3, 19, 228, 92, 47, 213, 122, 102, 219, 49, 81, 14, 110, 133, 8, 74, 65, 179, 237, 83, 208, 83, 84, 78, 171, 101 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 7, 31, 22, 11, 18, 114, DateTimeKind.Local).AddTicks(1854), new DateTime(2024, 7, 31, 22, 11, 18, 114, DateTimeKind.Local).AddTicks(1857), new byte[] { 52, 196, 242, 7, 74, 61, 240, 230, 14, 152, 199, 210, 174, 255, 105, 60, 243, 134, 122, 170, 15, 3, 52, 129, 106, 202, 185, 44, 65, 161, 205, 24 }, new byte[] { 240, 233, 87, 126, 155, 92, 97, 89, 0, 105, 224, 81, 84, 183, 55, 191, 130, 96, 33, 187, 143, 76, 69, 110, 128, 240, 43, 50, 50, 241, 84, 54, 211, 96, 203, 158, 231, 254, 5, 158, 129, 7, 143, 216, 184, 147, 46, 44, 233, 114, 196, 126, 125, 164, 68, 118, 190, 210, 138, 103, 139, 229, 70, 193 } });
        }
    }
}
