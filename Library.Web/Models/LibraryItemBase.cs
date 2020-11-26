using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{

    // When I choose to categorize the library items into "Digital Media" and "Non Digital Media",
    // I thought it would be nice to share the common properties. The two categories derives from this
    // class. I use "virtual" on the properties to be able to override them where needed. For example:
    // properties in Non Digital Media does not have to have "Runtime Minutes" as required, whilst in
    // DigitalMedia, this propery is required.

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

        [Display(Name = "Borrower date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BorrowDate { get; set; }
        public CategoryModel Category{ get; set; }
    }
}