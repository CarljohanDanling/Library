using Library.Engine.Enums;
using Library.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.ViewModels
{
    public class CreateEmployeeViewModel
    {
        public EmployeeModel Employee { get; set; }

        [Display(Name = "Manager")]
        public List<NonRegularEmployeeModel> NonRegularEmployee { get; set; }
        public EmployeeType EmployeeType{ get; set; }
    }
}