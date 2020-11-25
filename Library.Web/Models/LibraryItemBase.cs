using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class LibraryItemBase
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public bool IsBorrowable { get; set; }
        [Required]
        public LibraryItemType ItemType { get; set; }

        public virtual string Author { get; set; }
        public virtual int Pages { get; set; }
        public virtual int RunTimeMinutes { get; set; }

        public virtual string Borrower { get; set; }
        public DateTime? BorrowDate { get; set; }
        public CategoryModel Category{ get; set; }
    }
}