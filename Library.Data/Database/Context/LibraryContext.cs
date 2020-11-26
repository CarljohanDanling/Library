using Library.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Library.Data.Database.Context
{
    // This is where I set up the database with relations and also seed some dummy data.
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options) { }

        public DbSet<LibraryItem> LibraryItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; }

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
                    CategoryId = 3,
                    Title = "Jorden runt på 80 dagar",
                    Author = "James Verne",
                    Pages = 200,
                    RunTimeMinutes = null,
                    IsBorrowable = false,
                    Borrower = "Carina",
                    BorrowDate = new DateTime(2020, 06, 19, 14, 0, 0),
                    Type = "Book"
                });

            modelBuilder.Entity<LibraryItem>().HasData(
                new LibraryItem()
                {
                    Id = 2,
                    CategoryId = 1,
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
                    Type = "Dvd"
                });

            modelBuilder.Entity<LibraryItem>().HasData(
                new LibraryItem()
                {
                    Id = 4,
                    CategoryId = 2,
                    Title = "Granner ljuger",
                    RunTimeMinutes = 100,
                    IsBorrowable = true,
                    Borrower = null,
                    BorrowDate = null,
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

            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "Manager",
                    Salary = 13.8M,
                    IsCEO = false,
                    IsManager = true,
                    ManagerId = null
                });

            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    Id = 2,
                    FirstName = "Test",
                    LastName = "Regular",
                    Salary = 3.3750M,
                    IsCEO = false,
                    IsManager = false,
                    ManagerId = 1
                });

            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    Id = 3,
                    FirstName = "Test",
                    LastName = "CEO",
                    Salary = 16.3500M,
                    IsCEO = true,
                    IsManager = false,
                    ManagerId = null
                });

            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    Id = 4,
                    FirstName = "Test",
                    LastName = "Manager2",
                    Salary = 15.5250M,
                    IsCEO = false,
                    IsManager = true,
                    ManagerId = 1
                });

            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    Id = 5,
                    FirstName = "Test",
                    LastName = "Regular",
                    Salary = 4.5000M,
                    IsCEO = false,
                    IsManager = false,
                    ManagerId = 4
                });
        }
    }
}