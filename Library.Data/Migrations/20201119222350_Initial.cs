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
                    CategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
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
                values: new object[] { 1, "Adventure" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[] { 2, "Comedy" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[] { 3, "Horror" });

            migrationBuilder.InsertData(
                table: "LibraryItems",
                columns: new[] { "Id", "Author", "BorrowDate", "Borrower", "CategoryId", "IsBorrowable", "Pages", "RunTimeMinutes", "Title", "Type" },
                values: new object[] { 1, "James Verne", null, null, 1, true, 200, null, "Jorden runt på 80 dagar", "Book" });

            migrationBuilder.InsertData(
                table: "LibraryItems",
                columns: new[] { "Id", "Author", "BorrowDate", "Borrower", "CategoryId", "IsBorrowable", "Pages", "RunTimeMinutes", "Title", "Type" },
                values: new object[] { 2, "Kristina Apppelqvist", null, null, 2, true, 200, null, "De blå damerna", "Book" });

            migrationBuilder.InsertData(
                table: "LibraryItems",
                columns: new[] { "Id", "Author", "BorrowDate", "Borrower", "CategoryId", "IsBorrowable", "Pages", "RunTimeMinutes", "Title", "Type" },
                values: new object[] { 3, null, new DateTime(2020, 11, 5, 14, 0, 0, 0, DateTimeKind.Unspecified), "Pär", 2, true, null, 100, "Metallica", "AudioBook" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true,
                filter: "[CategoryName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryItems_CategoryId",
                table: "LibraryItems",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryItems");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
