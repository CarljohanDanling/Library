using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class ReferenceBookCreate
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int Pages { get; set; }

        [Required]
        public string Type = "ReferenceBook";

        [Required]
        public int CategoryId { get; set; }
    }
}