using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class AudioBookCreate
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Range(1, Double.PositiveInfinity)]
        public int RunTimeMinutes { get; set; }

        [Required]
        public string Type = "AudioBook";

        [Required]
        public int CategoryId { get; set; }
    }
}