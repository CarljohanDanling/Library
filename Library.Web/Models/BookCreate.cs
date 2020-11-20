using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class BookCreate
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int Pages { get; set; }

        [Required]
        public string Type = "Book";

        [Required]
        public int CategoryId { get; set; }
    }
}