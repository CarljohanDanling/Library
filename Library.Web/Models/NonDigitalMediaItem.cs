using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class NonDigitalMediaItem : LibraryItemBase
    {
        [Required]
        public override string Author { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public override int Pages { get; set; }

        [Required]
        [Display(Name = "Type")]
        public NonDigitalMediaItemType NonDigitalMediaItemType { get; set; }
    }
}