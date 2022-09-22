using Microsoft.EntityFrameworkCore.Migrations;

namespace MagazinOnline.DataAcces.Migrations
{
    public partial class AddCompaniiToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companii",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Oras = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tara = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodPostal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NrTelefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAutorizat = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companii", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companii");
        }
    }
}
