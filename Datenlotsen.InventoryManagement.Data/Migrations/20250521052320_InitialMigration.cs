using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Datenlotsen.InventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StockQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01a5b6ab-91ef-4c92-903d-3e2a9af332fd"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5200), "Grains", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5200) },
                    { new Guid("0b705731-1ba7-43da-8bfa-380f0709eea6"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5198), "Seafood", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5198) },
                    { new Guid("1d30f3d1-9b44-42b0-ae88-1d88774762e0"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5180), "Fruits", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5181) },
                    { new Guid("2c0e21b5-6817-437c-a3db-aa7ca32ce124"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5204), "Baking", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5204) },
                    { new Guid("35a82aa2-9891-417e-833e-846371aca063"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5210), "Packaged Foods", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5211) },
                    { new Guid("38844051-c658-48c2-9fa6-3aa0e4378df2"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5183), "Vegetables", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5183) },
                    { new Guid("46cfc137-f3d7-482a-b2a7-4d124b910a83"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5187), "Meat", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5187) },
                    { new Guid("68172734-18b4-4e8d-8ec9-82f7c73570d9"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5208), "Canned Foods", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5208) },
                    { new Guid("b697caf5-3756-4af6-bfb9-e9c32b5180d4"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5206), "Frozen Foods", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5206) },
                    { new Guid("c0f13d5f-b34b-4134-a09e-f8b559c2e07d"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5174), "Beverages - Alcoholic", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5175) },
                    { new Guid("d6374f57-090e-4ebb-a254-c3f8ff6d8241"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5202), "Condiments", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5202) },
                    { new Guid("ea2f8cd9-990b-4f1a-b5e3-c62f405c43ed"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5178), "Snacks", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5179) },
                    { new Guid("ee65850e-861c-445f-99a3-a794c72a7c2c"), new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5185), "Dairy", new DateTime(2025, 5, 21, 5, 23, 20, 25, DateTimeKind.Utc).AddTicks(5185) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_CategoryId",
                table: "InventoryItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_Name",
                table: "InventoryItems",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
