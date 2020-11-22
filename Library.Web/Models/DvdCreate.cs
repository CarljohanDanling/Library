using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class DvdCreate : LibraryItemBase
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public override int RunTimeMinutes { get; set; }
    }
}