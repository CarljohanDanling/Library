using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class BookEdit
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Author { get; set; }
        
        [Required]
        public int Pages { get; set; }
        
        [Required]
        public string Type { get; set; }
        
        [Required]
        public bool IsBorrowable { get; set; }

        public string Borrower { get; set; }

        public DateTime BorrowDate { get; set; }
        
        [Required]
        public int CategoryId { get; set; }

        public List<CategoryModel> Categories { get; set; }
    }
}
