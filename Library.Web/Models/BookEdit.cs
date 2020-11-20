using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class BookEdit
    {
        [Required]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Pages { get; set; }

        public string Type { get; set; }

        public bool IsBorrowable { get; set; }

        public string Borrower { get; set; }

        public DateTime BorrowDate { get; set; }

        public int CategoryId { get; set; }

        public List<CategoryModel> Categories { get; set; }
    }
}
