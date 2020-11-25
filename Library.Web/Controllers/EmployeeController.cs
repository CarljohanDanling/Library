using AutoMapper;
using Library.Engine.Dtos;
using Library.Engine.Enums;
using Library.Engine.Interface;
using Library.Web.Models;
using Library.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
                if (viewModel.EmployeeType == EmployeeType.Employee)
                {
                    var employee = _mapper.Map<EmployeeModel, EmployeeDto>(viewModel.Employee);
                    await _employeeService.CreateEmployee(employee);
                }

                else if (viewModel.EmployeeType == EmployeeType.Manager)
                {
                    var employee = _mapper.Map<EmployeeModel, EmployeeDto>(viewModel.Employee);
                    employee.IsManager = true;
                    await _employeeService.CreateEmployee(employee);
                }

                else
                {
                    if (await _employeeService.IsThereAnyExistingCeo())
                    {
                        ModelState.AddModelError("OnlyOneCEOError", "Error! There can only exist one CEO");
                        return View(viewModel);
                    }

                    var employee = _mapper.Map<EmployeeModel, EmployeeDto>(viewModel.Employee);
                    employee.IsCEO = true;
                    await _employeeService.CreateEmployee(employee);
                }

                return RedirectToAction("Index", "Employee");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployee(id);
            var employeeMapped = _mapper.Map<EmployeeModel>(employee);

            return View(employeeMapped);
        }

        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployee(id);
            var employeeMapped = _mapper.Map<EmployeeModel>(employee);

            return View(employeeMapped);
        }

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
    }
}