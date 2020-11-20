using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class DvdCreate
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Range(1, Double.PositiveInfinity)]
        public int RunTimeMinutes { get; set; }

        [Required]
        public string Type = "Dvd";

        [Required]
        public int CategoryId { get; set; }
    }
}