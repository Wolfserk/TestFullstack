using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestFullstack.Server.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemPrice",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ItemsCount",
                table: "OrderItems");
        }
    }
}
