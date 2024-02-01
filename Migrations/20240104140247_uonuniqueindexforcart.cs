using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessoriesWebsite.Migrations
{
    public partial class uonuniqueindexforcart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_carts_userId",
                table: "carts");

            migrationBuilder.CreateIndex(
                name: "IX_carts_userId",
                table: "carts",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_carts_userId",
                table: "carts");

            migrationBuilder.CreateIndex(
                name: "IX_carts_userId",
                table: "carts",
                column: "userId",
                unique: true);
        }
    }
}
