using Library.Engine.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class EmployeeModel
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public bool IsManager { get; set; }

        [Required]
        public bool IsCEO { get; set; }

        [Required]
        [Range(1, 10)]
        public int Rank { get; set; }

        [Display(Name = "Managed by")]
        public string ManagedBy { get; set; }

        [Display(Name = "Employee type")]
        public EmployeeType EmployeeType { get; set; }

        public decimal Salary { get; set; }
        public int? ManagerId { get; set; }
        public string Name { get; set; }
    }
}