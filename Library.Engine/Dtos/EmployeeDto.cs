using Library.Data.Database.Models;
using Library.Engine.Enums;

namespace Library.Engine.Dtos
{
    public class EmployeeDto : Employee
    {
        public EmployeeType EmployeeType { get; set; }
        public string ManagedBy { get; set; }
        public int Rank { get; set; }
    }
}