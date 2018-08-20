using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StoreHouse.Data.Migrations
{
    public partial class osztalyok : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partner",
                columns: table => new
                {
                    PartnerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 100, nullable: true),
                    email = table.Column<string>(maxLength: 40, nullable: true),
                    name = table.Column<string>(maxLength: 40, nullable: true),
                    phone = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partner", x => x.PartnerID);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    currency = table.Column<int>(nullable: true),
                    description = table.Column<string>(maxLength: 40, nullable: true),
                    name = table.Column<string>(nullable: false),
                    purchase_price = table.Column<int>(nullable: false),
                    sale_price = table.Column<int>(nullable: false),
                    stock = table.Column<int>(nullable: false),
                    url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "Export",
                columns: table => new
                {
                    ExportID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PartnerID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    quantity = table.Column<int>(nullable: false),
                    sale_price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Export", x => x.ExportID);
                    table.ForeignKey(
                        name: "FK_Export_Partner_PartnerID",
                        column: x => x.PartnerID,
                        principalTable: "Partner",
                        principalColumn: "PartnerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Export_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Import",
                columns: table => new
                {
                    ImportID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PartnerID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    quantity = table.Column<int>(nullable: false),
                    sale_price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Import", x => x.ImportID);
                    table.ForeignKey(
                        name: "FK_Import_Partner_PartnerID",
                        column: x => x.PartnerID,
                        principalTable: "Partner",
                        principalColumn: "PartnerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Import_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Export_PartnerID",
                table: "Export",
                column: "PartnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Export_ProductID",
                table: "Export",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Import_PartnerID",
                table: "Import",
                column: "PartnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Import_ProductID",
                table: "Import",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Export");

            migrationBuilder.DropTable(
                name: "Import");

            migrationBuilder.DropTable(
                name: "Partner");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
