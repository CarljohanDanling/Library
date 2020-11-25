using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Data.Database.Models
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }

        public ICollection<LibraryItem> LibraryItems{ get; set; }
    }
}