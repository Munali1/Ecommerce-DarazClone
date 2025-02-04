using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "tbl_Order",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Cart_Id",
                table: "tbl_Order",
                newName: "Product_ID");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "tbl_Cart",
                newName: "Quantity");

            migrationBuilder.AddColumn<int>(
                name: "Customer_ID",
                table: "tbl_Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "tbl_Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "tbl_Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "tbl_Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Order_Customer_ID",
                table: "tbl_Order",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Order_Product_ID",
                table: "tbl_Order",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Cart_Customer_ID",
                table: "tbl_Cart",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Cart_Product_ID",
                table: "tbl_Cart",
                column: "Product_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Cart_tbl_Customer_Customer_ID",
                table: "tbl_Cart",
                column: "Customer_ID",
                principalTable: "tbl_Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Cart_tbl_Product_Product_ID",
                table: "tbl_Cart",
                column: "Product_ID",
                principalTable: "tbl_Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Order_tbl_Customer_Customer_ID",
                table: "tbl_Order",
                column: "Customer_ID",
                principalTable: "tbl_Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Order_tbl_Product_Product_ID",
                table: "tbl_Order",
                column: "Product_ID",
                principalTable: "tbl_Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Cart_tbl_Customer_Customer_ID",
                table: "tbl_Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Cart_tbl_Product_Product_ID",
                table: "tbl_Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Order_tbl_Customer_Customer_ID",
                table: "tbl_Order");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Order_tbl_Product_Product_ID",
                table: "tbl_Order");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Order_Customer_ID",
                table: "tbl_Order");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Order_Product_ID",
                table: "tbl_Order");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Cart_Customer_ID",
                table: "tbl_Cart");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Cart_Product_ID",
                table: "tbl_Cart");

            migrationBuilder.DropColumn(
                name: "Customer_ID",
                table: "tbl_Order");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "tbl_Order");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "tbl_Order");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "tbl_Order");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "tbl_Order",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Product_ID",
                table: "tbl_Order",
                newName: "Cart_Id");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "tbl_Cart",
                newName: "quantity");
        }
    }
}
