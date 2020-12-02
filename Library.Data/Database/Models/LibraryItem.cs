using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Data.Database.Models
{
    // I'm making my own interpretation here. The requirements says "Author" and "Borrower" cannot
    // be null. But, all items doesnt have an author and when creating an item you dont have
    // a borrower either.
    public class LibraryItem
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Title { get; set; }

        #nullable enable
        public string? Author { get; set; }
        #nullable disable

        public int? Pages { get; set; }
        public int? RunTimeMinutes { get; set; }

        [Required]
        public bool IsBorrowable { get; set; }

        #nullable enable
        public string? Borrower { get; set; }
        #nullable disable

        public DateTime? BorrowDate { get; set; }

        [Required]
        public string Type { get; set; }

        public Category Category { get; set; }
    }
}