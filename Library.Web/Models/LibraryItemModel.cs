using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class LibraryItemModel
    {
        public int Id { get; set; }
        public CategoryModel Category { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public int RunTimeMinutes { get; set; }
        public bool IsBorrowable { get; set; }
        public string Borrower { get; set; }

        [Display(Name = "Borrower Date")]
        public DateTime BorrowerDate { get; set; }
        public string Type { get; set; }
    }
}