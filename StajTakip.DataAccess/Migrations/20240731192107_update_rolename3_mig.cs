using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class update_rolename3_mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserOperationClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "OperationClaimId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 7, 31, 22, 21, 7, 537, DateTimeKind.Local).AddTicks(5099), new DateTime(2024, 7, 31, 22, 21, 7, 537, DateTimeKind.Local).AddTicks(5101), new byte[] { 194, 179, 166, 174, 46, 136, 234, 145, 228, 251, 43, 101, 231, 149, 86, 114, 57, 87, 55, 22, 202, 50, 103, 11, 153, 228, 15, 217, 92, 110, 73, 249 }, new byte[] { 223, 228, 154, 191, 208, 89, 61, 90, 60, 211, 7, 214, 145, 199, 177, 178, 204, 112, 219, 45, 85, 235, 146, 222, 46, 6, 221, 146, 58, 93, 77, 241, 73, 190, 242, 158, 173, 158, 177, 79, 140, 53, 168, 213, 24, 106, 115, 253, 41, 190, 138, 226, 179, 108, 117, 36, 162, 233, 54, 82, 243, 41, 206, 103 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserOperationClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "OperationClaimId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 7, 31, 22, 19, 29, 366, DateTimeKind.Local).AddTicks(7284), new DateTime(2024, 7, 31, 22, 19, 29, 366, DateTimeKind.Local).AddTicks(7285), new byte[] { 187, 3, 211, 40, 178, 50, 2, 94, 206, 88, 249, 56, 8, 156, 55, 72, 96, 102, 233, 211, 37, 253, 222, 164, 238, 176, 223, 233, 138, 56, 50, 234 }, new byte[] { 3, 113, 115, 144, 36, 250, 237, 243, 226, 200, 255, 162, 254, 231, 38, 237, 65, 186, 154, 240, 220, 92, 232, 190, 186, 7, 220, 180, 200, 138, 131, 70, 143, 97, 156, 55, 58, 73, 3, 19, 228, 92, 47, 213, 122, 102, 219, 49, 81, 14, 110, 133, 8, 74, 65, 179, 237, 83, 208, 83, 84, 78, 171, 101 } });
        }
    }
}
