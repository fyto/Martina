using Microsoft.EntityFrameworkCore.Migrations;

namespace Martina.API.Migrations
{
    public partial class AddDiseaseType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeseaseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeseaseTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeseaseTypes_Description",
                table: "DeseaseTypes",
                column: "Description",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeseaseTypes");
        }
    }
}
