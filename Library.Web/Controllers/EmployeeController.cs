using AutoMapper;
using Library.Engine.Dtos;
using Library.Engine.Enums;
using Library.Engine.Interface;
using Library.Web.Models;
using Library.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Library.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployees();
            var employeesMapped = _mapper.Map<List<EmployeeModel>>(employees);

            var viewModel = new EmployeeViewModel
            {
                Employees = employeesMapped
            };

            return View(viewModel);
        }

        // "GetNonRegularEmployees" call is used to be able to assign an employee with a manager.
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var managers = await _employeeService.GetNonRegularEmployees();
            var managersMapped = _mapper.Map<List<NonRegularEmployeeModel>>(managers);

            var viewModel = new CreateEmployeeViewModel
            {
                NonRegularEmployee = managersMapped,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<EmployeeModel, EmployeeDto>(viewModel.Employee);

                try
                {
                    await _employeeService.CreateEmployee(employee, viewModel.EmployeeType);
                }

                catch (InvalidOperationException)
                {
                    ModelState.AddModelError("OnlyOneCEOError", "Error! There can only exist one CEO");
                    return View(viewModel);
                }

                return RedirectToAction("Index", "Employee");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetching managers and ceo, they are used in the view to assign to employees.
            var nonRegularEmployees = await _employeeService.GetNonRegularEmployees();
            var nonRegularEmployeesMapped = _mapper.Map<List<NonRegularEmployeeModel>>(nonRegularEmployees);

            var employee = await _employeeService.GetEmployee(id);
            var employeeMapped = _mapper.Map<EmployeeModel>(employee);

            var viewModel = new EditEmployeeViewModel
            {
                Employee = employeeMapped,
                NonRegularEmployees = nonRegularEmployeesMapped
            };

            return View(viewModel);
        }

        // Im not very happy with this method. First, its too big and secondly it screams "DRY".
        // The thing is, I need to take different actions depending on the empoloyee type. I found it
        // hard to make a generic version. The "isChangingEmployeeType" is used to check if the employee
        // wants to make a shift from manager -> CEO, just as an example.
        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeViewModel viewModel, EmployeeType currentType)
        {
            var employee = _mapper.Map<EmployeeModel, EmployeeDto>(viewModel.Employee);
            var newType = viewModel.Employee.EmployeeType;
            var isChangingEmployeeType = currentType != newType;

            if (ModelState.IsValid)
            {
                if (isChangingEmployeeType)
                {
                    switch (currentType)
                    {
                        case EmployeeType.Employee:
                            var regularEmployeeSuccess = await RegularEmployeeHandler(currentType, newType, employee);
                            if (regularEmployeeSuccess) return RedirectToAction("Index", "Employee");
                            viewModel.Employee.EmployeeType = currentType;
                            return View(viewModel);

                        case EmployeeType.Manager:
                            var managerEmployeeSuccess = await ManagerHandler(currentType, newType, employee);
                            if (managerEmployeeSuccess) return RedirectToAction("Index", "Employee");
                            viewModel.Employee.EmployeeType = currentType;
                            return View(viewModel);

                        default:
                            await CEOHandler(currentType, newType, employee);
                            return RedirectToAction("Index", "Employee");
                    }
                }

                switch (currentType)
                {
                    case EmployeeType.Employee:
                        await _employeeService.EditEmployee(employee);
                        return RedirectToAction("Index", "Employee");

                    case EmployeeType.Manager:
                        employee.IsManager = true;
                        await _employeeService.EditEmployee(employee);
                        return RedirectToAction("Index", "Employee");

                    case EmployeeType.CEO:
                        employee.IsCEO = true;
                        await _employeeService.EditEmployee(employee);
                        return RedirectToAction("Index", "Employee");
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployee(id);
            var employeeMapped = _mapper.Map<EmployeeModel>(employee);

            return View(employeeMapped);
        }

        // In this method I first check to see if the employee-to-delete is
        // not managing any other.
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeModel employee)
        {
            var isManagingOther = await _employeeService.IsManagingOther(employee.Id);

            if (isManagingOther)
            {
                ModelState.AddModelError("IsManagingAnotherError", "Can't delete, this employee is managing another one");
                return View(employee);
            }

            var ableToDelete = await _employeeService.DeleteEmployee(employee.Id);

            if (ableToDelete)
            {
                return RedirectToAction("Index", "Employee");
            }

            return View(employee);
        }

        // This is the handler for the regular employee. I differentiate what the users wants to do by checking
        // the current employee type with what the user chose. I think the variable name "employeeToManager" is quite
        // self explanatory. I chose to make the regular employee able to upgrade to manager or ceo.
        private async Task<bool> RegularEmployeeHandler(EmployeeType currentType, EmployeeType newType, EmployeeDto employee)
        {
            var regularEmployeeToManager = currentType == EmployeeType.Employee && newType == EmployeeType.Manager;

            if (regularEmployeeToManager)
            {
                employee.IsManager = true;
                await _employeeService.EditEmployee(employee);
                return true;
            }

            // If employee wants to be a CEO, I need to check if there already exist one.
            if (await _employeeService.IsThereAnyExistingCeo())
            {
                ModelState.AddModelError("OnlyOneCEOError", "Error! There can only exist one CEO");
                return false;
            }

            employee.IsCEO = true;
            await _employeeService.EditEmployee(employee);
            return true;
        }

        // This is the handler for the managers. As in the previous method is differentiate the user's action and
        // try to catch errors. I took the decision that a Manager cannot downgrade to a regular employee.
        private async Task<bool> ManagerHandler(EmployeeType currentType, EmployeeType newType, EmployeeDto employee)
        {
            var managerToEmployee = currentType == EmployeeType.Manager && newType == EmployeeType.Employee;

            if (managerToEmployee)
            {
                ModelState.AddModelError("ManagerToRegularEmployeeError", "Cannot change role from manager to regular employee");
                return false;
            }

            if (await _employeeService.IsThereAnyExistingCeo())
            {
                ModelState.AddModelError("OnlyOneCEOError", "Error! There can only exist one CEO");
                return false;
            }

            await _employeeService.ClearManagerIdFromEmployees(employee.Id);
            employee.IsCEO = true;
            employee.ManagerId = null;
            await _employeeService.EditEmployee(employee);
            return true;
        }

        // This is the handler for the CEO. I added support for the CEO to downgrade
        // to manager or even regular employee.
        private async Task CEOHandler(EmployeeType currentType, EmployeeType newType, EmployeeDto employee)
        {
            var ceoToManager = currentType == EmployeeType.CEO && newType == EmployeeType.Manager;

            if (ceoToManager)
            {
                employee.IsManager = true;
                await _employeeService.EditEmployee(employee);
                return;
            }

            await _employeeService.ClearManagerIdFromEmployees(employee.Id);
            var managerId = await _employeeService.GetManagersId();

            // Assigns an manager, because regular employee, you need someone to report to.
            employee.ManagerId = managerId;
            await _employeeService.EditEmployee(employee);
            return;
        }
    }
}