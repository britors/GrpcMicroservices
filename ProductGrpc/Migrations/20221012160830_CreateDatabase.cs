using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductGrpc.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatuses", x => x.Id);
                });


            migrationBuilder.InsertData(table: "ProductStatuses",
                                        columns: new[] { "Id", "Name", "IsDeleted", "CreatedAt" },
                                        values: new object[] { 1, "INSTOCK", false, DateTime.UtcNow });

            migrationBuilder.InsertData(table: "ProductStatuses",
                                        columns: new[] { "Id", "Name", "IsDeleted", "CreatedAt" },
                                        values: new object[] { 2, "LOW", false, DateTime.UtcNow });

            migrationBuilder.InsertData(table: "ProductStatuses",
                                        columns: new[] { "Id", "Name", "IsDeleted", "CreatedAt" },
                                        values: new object[] { 3, "NONE", false, DateTime.UtcNow });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatuses_Name",
                table: "ProductStatuses",
                column: "Name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatuses_IsDeleted",
                table: "ProductStatuses",
                column: "IsDeleted"
            );


            migrationBuilder.CreateTable(
                    name: "Products",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                        Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                        Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Price = table.Column<float>(type: "real", nullable: false),
                        StatusId = table.Column<int>(type: "smallint", nullable: false),
                        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Products", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Products_ProductStatuses_StatusId",
                            column: x => x.StatusId,
                            principalTable: "ProductStatuses",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

            migrationBuilder.CreateIndex(
                name: "IX_Products_StatusId",
                table: "Products",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsDeleted",
                table: "Products",
                column: "IsDeleted"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductStatuses");
        }
    }
}
