using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        public virtual string Author { get; set; }
        public virtual int Pages { get; set; }
        public virtual int RunTimeMinutes { get; set; }
        public virtual string Borrower { get; set; }
        public DateTime? BorrowDate { get; set; }

        [Required]
        public bool IsBorrowable { get; set; }

        [Required]
        public LibraryItemType ItemType { get; set; }

        



        public CategoryModel Category{ get; set; }
    }
}