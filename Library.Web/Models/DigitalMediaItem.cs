using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class DigitalMediaItem : LibraryItemBase
    {
        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Length (minutes)")]
        public override int RunTimeMinutes { get; set; }

        [Required]
        [Display(Name = "Type")]
        public DigitalMediaItemType DigitalMediaItemType { get; set; }
    }
}