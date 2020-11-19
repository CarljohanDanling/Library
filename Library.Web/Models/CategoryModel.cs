using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class CategoryModel
    {
        [Required]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cannot be empty")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
    }
}