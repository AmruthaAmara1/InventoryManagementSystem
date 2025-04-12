using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionPricingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Reports",
                newName: "TotalProfit");

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "GeneratedDate",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "GeneratedDate",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "TotalProfit",
                table: "Reports",
                newName: "Date");
        }
    }
}
