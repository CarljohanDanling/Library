using Library.Web.Models;
using System.Collections.Generic;

namespace Library.Web.ViewModels
{
    public class EmployeeViewModel
    {
        public List<EmployeeModel> Employees { get; set; }
        public EmployeeModel Employee{ get; set; }
    }
}