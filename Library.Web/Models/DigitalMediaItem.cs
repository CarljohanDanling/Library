using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class DigitalMediaItem : LibraryItemBase
    {
        [Required]
        [Range(1, int.MaxValue)]
        public override int RunTimeMinutes { get; set; }

        [Required]
        [Display(Name = "Type")]
        public DigitalMediaItemType DigitalMediaItemType { get; set; }
    }
}