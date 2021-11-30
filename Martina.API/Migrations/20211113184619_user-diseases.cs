using Microsoft.EntityFrameworkCore.Migrations;

namespace Martina.API.Migrations
{
    public partial class userdiseases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deseases_AspNetUsers_UserId",
                table: "Deseases");

            migrationBuilder.DropIndex(
                name: "IX_Deseases_UserId",
                table: "Deseases");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Deseases");

            migrationBuilder.CreateTable(
                name: "UsersDiseases",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DiseaseId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDiseases", x => new { x.UserId, x.DiseaseId });
                    table.ForeignKey(
                        name: "FK_UsersDiseases_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersDiseases_Deseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Deseases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersDiseases_DiseaseId",
                table: "UsersDiseases",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDiseases_UserId1",
                table: "UsersDiseases",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersDiseases");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Deseases",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deseases_UserId",
                table: "Deseases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deseases_AspNetUsers_UserId",
                table: "Deseases",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
