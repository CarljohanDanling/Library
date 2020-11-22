using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class BookCreate : LibraryItemBase
    {
        [Required]
        public override string Author { get; set; }

        [Required]
        public override int Pages { get; set; }
    }
}