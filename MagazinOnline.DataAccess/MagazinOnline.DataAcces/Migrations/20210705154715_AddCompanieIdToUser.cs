using Microsoft.EntityFrameworkCore.Migrations;

namespace MagazinOnline.DataAcces.Migrations
{
    public partial class AddCompanieIdToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanieId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanieId",
                table: "AspNetUsers",
                column: "CompanieId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companii_CompanieId",
                table: "AspNetUsers",
                column: "CompanieId",
                principalTable: "Companii",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companii_CompanieId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanieId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanieId",
                table: "AspNetUsers");
        }
    }
}
