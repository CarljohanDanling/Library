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

                catch (InvalidOperationException ex)
                {
                    GenerateModelError(ex);
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

        // This method calls EmployeeCoordinator (service layer). If an invalid operation takes place
        // in the service layer, it bubbles up to this method and applies model state error.
        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeViewModel viewModel, EmployeeType currentType)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<EmployeeModel, EmployeeDto>(viewModel.Employee);

                try
                {
                    await _employeeService.EmployeeCoordinator(employee, currentType, viewModel.Employee.EmployeeType);
                }

                catch (InvalidOperationException ex)
                {
                    GenerateModelError(ex);
                    // Sets the current type so the form in the view shows the correct one.
                    viewModel.Employee.EmployeeType = currentType;
                    return View(viewModel);
                }

                return RedirectToAction("Index", "Employee");
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

        // Helper method generating modelstate errors.
        private void GenerateModelError(InvalidOperationException ex)
        {
            if (ex.Message == "OnlyOneCEOError")
            {
                ModelState.AddModelError("OnlyOneCEOError", "Error! There can only exist one CEO");
                return;
            }

            if (ex.Message == "ManagerToRegularEmployeeError")
            {
                ModelState.AddModelError("ManagerToRegularEmployeeError", "Cannot change role from manager to regular employee");
                return;
            }
        }
    }
}