using Library.Engine.Enums;
using Library.Web.Models;
using System.Collections.Generic;

namespace Library.Web.ViewModels
{
    public class EditEmployeeViewModel
    {
        public EmployeeModel Employee { get; set; }
        public List<NonRegularEmployeeModel> NonRegularEmployees { get; set; }
        public EmployeeType TypeOfEmployee { get; set; }
    }
}