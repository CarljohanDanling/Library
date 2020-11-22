using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class AudioBookCreate : LibraryItemBase
    {
        [Required]
        [Range(1, Double.PositiveInfinity)]
        public override int RunTimeMinutes { get; set; }
    }
}