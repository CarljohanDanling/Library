using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsCEO = table.Column<bool>(nullable: false),
                    IsManager = table.Column<bool>(nullable: false),
                    ManagerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Pages = table.Column<int>(nullable: true),
                    RunTimeMinutes = table.Column<int>(nullable: true),
                    IsBorrowable = table.Column<bool>(nullable: false),
                    Borrower = table.Column<string>(nullable: true),
                    BorrowDate = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Adventure" },
                    { 2, "Comedy" },
                    { 3, "Horror" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "FirstName", "IsCEO", "IsManager", "LastName", "ManagerId", "Salary" },
                values: new object[,]
                {
                    { 1, "Test", false, true, "Manager", null, 13.8m },
                    { 2, "Test", false, false, "Regular", 1, 3.3750m },
                    { 3, "Test", true, false, "CEO", null, 16.3500m },
                    { 4, "Test", false, true, "Manager2", 1, 15.5250m },
                    { 5, "Test", false, false, "Regular", 4, 4.5000m }
                });

            migrationBuilder.InsertData(
                table: "LibraryItems",
                columns: new[] { "Id", "Author", "BorrowDate", "Borrower", "CategoryId", "IsBorrowable", "Pages", "RunTimeMinutes", "Title", "Type" },
                values: new object[,]
                {
                    { 2, "Kristina Apppelqvist", null, null, 1, true, 200, null, "De blå damerna", "Book" },
                    { 3, null, new DateTime(2020, 11, 5, 14, 0, 0, 0, DateTimeKind.Unspecified), "Pär", 2, false, null, 100, "Metallica", "Dvd" },
                    { 4, null, null, null, 2, true, null, 100, "Granner ljuger", "AudioBook" },
                    { 1, "James Verne", new DateTime(2020, 6, 19, 14, 0, 0, 0, DateTimeKind.Unspecified), "Carina", 3, false, 200, null, "Jorden runt på 80 dagar", "Book" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryItems_CategoryId",
                table: "LibraryItems",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "LibraryItems");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
