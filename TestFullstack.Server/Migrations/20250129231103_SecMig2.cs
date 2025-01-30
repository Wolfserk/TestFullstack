using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestFullstack.Server.Migrations
{
    /// <inheritdoc />
    public partial class SecMig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Items_ItemId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Items_ItemId1",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ItemId1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ItemId1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ItemPrice",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ItemsCount",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Items_ItemId",
                table: "OrderItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Items_ItemId",
                table: "OrderItems");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId1",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ItemsCount",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ItemId1",
                table: "OrderItems",
                column: "ItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Items_ItemId",
                table: "OrderItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Items_ItemId1",
                table: "OrderItems",
                column: "ItemId1",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
