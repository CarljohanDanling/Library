using Library.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data.Database.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options) { }

        public DbSet<LibraryItem> LibraryItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.LibraryItems)
                .WithOne(l => l.Category)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();


            // SEED DATA
            modelBuilder.Entity<LibraryItem>().HasData(
                new LibraryItem()
                {
                    Id = 1,
                    CategoryId = 1,
                    Title = "Jorden runt på 80 dagar",
                    Author = "James Verne",
                    Pages = 200,
                    RunTimeMinutes = null,
                    IsBorrowable = true,
                    Borrower = null,
                    BorrowDate = null,
                    Type = "Book"
                });

            modelBuilder.Entity<LibraryItem>().HasData(
                new LibraryItem()
                {
                    Id = 2,
                    CategoryId = 2,
                    Title = "De blå damerna",
                    Author = "Kristina Apppelqvist",
                    Pages = 200,
                    IsBorrowable = true,
                    Borrower = null,
                    BorrowDate = null,
                    Type = "Book"
                });

            modelBuilder.Entity<LibraryItem>().HasData(
                new LibraryItem()
                {
                    Id = 3,
                    CategoryId = 2,
                    Title = "Metallica",
                    RunTimeMinutes = 100,
                    IsBorrowable = false,
                    Borrower = "Pär",
                    BorrowDate = new DateTime(2020, 11, 05, 14, 00, 00),
                    Type = "AudioBook"
                });

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    CategoryName = "Adventure"
                });

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 2,
                    CategoryName = "Comedy"
                });

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 3,
                    CategoryName = "Horror"
                });
        }
    }
}
