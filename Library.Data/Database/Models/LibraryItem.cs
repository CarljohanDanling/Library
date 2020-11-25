using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Data.Database.Models
{
    public class LibraryItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Author { get; set; }
        public int? Pages { get; set; }
        public int? RunTimeMinutes { get; set; }

        [Required]
        public bool IsBorrowable { get; set; }

        #nullable enable
        public string? Borrower { get; set; }
        public DateTime? BorrowDate { get; set; }
        public string? Type { get; set; }
    }
}