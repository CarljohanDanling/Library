using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class ReferenceBookEdit
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

        [Required]
        public int CategoryId { get; set; }

        public List<CategoryModel> Categories { get; set; }
    }
}