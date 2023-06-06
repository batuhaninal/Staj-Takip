using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajTakip.DataAccess.Migrations
{
    public partial class admin_student_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminStudentRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminUserId = table.Column<int>(type: "int", nullable: true),
                    StudentUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminStudentRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminStudentRelations_AdminUsers_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "AdminUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdminStudentRelations_StudentUsers_StudentUserId",
                        column: x => x.StudentUserId,
                        principalTable: "StudentUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminStudentRelations_AdminUserId",
                table: "AdminStudentRelations",
                column: "AdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminStudentRelations_StudentUserId",
                table: "AdminStudentRelations",
                column: "StudentUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminStudentRelations");
        }
    }
}
