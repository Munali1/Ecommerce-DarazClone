using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class ProductandCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_Product_CategoryId",
                table: "tbl_Product",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Product_tbl_Category_CategoryId",
                table: "tbl_Product",
                column: "CategoryId",
                principalTable: "tbl_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Product_tbl_Category_CategoryId",
                table: "tbl_Product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Product_CategoryId",
                table: "tbl_Product");
        }
    }
}
