using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class AudioBookEdit
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public LibraryItemType LibraryItemType { get; set; }

        [Required]
        public string Type { get; set; }
        
        [Required]
        public int RunTimeMinutes { get; set; }

        [Required]
        public bool IsBorrowable { get; set; }
        
        public string Borrower { get; set; }

        public DateTime BorrowDate { get; set; }
        
        [Required]
        public int CategoryId { get; set; }

        public List<CategoryModel> Categories{ get; set; }
    }
}
