﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class Book : LibraryItemBase
    {
        [Required]
        public override string Author { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public override int Pages { get; set; }

        public List<CategoryModel> Categories { get; set; }
    }
}