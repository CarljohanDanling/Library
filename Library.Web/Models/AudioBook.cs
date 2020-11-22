﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class AudioBook : LibraryItemBase
    {
        [Required]
        [Range(1, int.MaxValue)]
        public override int RunTimeMinutes { get; set; }

        public List<CategoryModel> Categories { get; set; }
    }
}