using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data.Database.Models
{
    public class Employee
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Salary { get; set; }
        [Required]
        public bool IsCEO { get; set; }
        [Required]
        public bool IsManager { get; set; }

        public int? ManagerId { get; set; }
    }
}