using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class seed_user_data_mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OperationClaims",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Admin" });

            migrationBuilder.InsertData(
                table: "UserOperationClaims",
                columns: new[] { "Id", "OperationClaimId", "UserId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedByName", "CreatedDate", "Email", "IsActive", "IsDeleted", "ModifiedByName", "ModifiedDate", "Note", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[] { 1, "Initial", new DateTime(2024, 7, 31, 22, 6, 58, 891, DateTimeKind.Local).AddTicks(9880), "admin@admin.com", true, false, "Initial", new DateTime(2024, 7, 31, 22, 6, 58, 891, DateTimeKind.Local).AddTicks(9881), "Initial", new byte[] { 59, 145, 160, 153, 215, 50, 194, 69, 163, 130, 252, 245, 156, 23, 103, 58, 50, 189, 163, 75, 201, 165, 70, 147, 35, 230, 134, 41, 195, 249, 121, 240 }, new byte[] { 7, 158, 242, 74, 12, 7, 222, 2, 164, 58, 54, 29, 168, 196, 51, 174, 158, 164, 148, 136, 92, 118, 221, 11, 217, 255, 151, 73, 191, 123, 240, 85, 54, 38, 120, 140, 16, 29, 44, 106, 76, 224, 25, 25, 193, 237, 158, 155, 207, 61, 133, 98, 152, 93, 248, 39, 2, 203, 27, 133, 146, 172, 234, 45 }, "adminuser" });

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "FirstName", "IsCompany", "LastName", "UserId" },
                values: new object[] { 1, "Admin", false, "Admin", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserOperationClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
