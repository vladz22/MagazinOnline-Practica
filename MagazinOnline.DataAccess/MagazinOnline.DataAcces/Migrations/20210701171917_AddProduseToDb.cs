using Microsoft.EntityFrameworkCore.Migrations;

namespace MagazinOnline.DataAcces.Migrations
{
    public partial class AddProduseToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pret = table.Column<double>(type: "float", nullable: false),
                    Pret5 = table.Column<double>(type: "float", nullable: false),
                    ImagineUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategorieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produse_Categoriile_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categoriile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produse_CategorieId",
                table: "Produse",
                column: "CategorieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produse");
        }
    }
}
