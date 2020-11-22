using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class CategoryModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
    }
}