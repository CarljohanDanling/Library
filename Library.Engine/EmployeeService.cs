using AutoMapper;
using Library.Data.Database.Models;
using Library.Data.Interfaces;
using Library.Engine.Dtos;
using Library.Engine.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Library.Engine.Enums;
using System;

namespace Library.Engine
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ISalaryCalculator _salaryCalculator;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(ISalaryCalculator salaryCalculator, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _salaryCalculator = salaryCalculator;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }


        public async Task<EmployeeDto> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployee(id);
            var employeeMapped = _mapper.Map<EmployeeDto>(employee);

            if (employeeMapped.EmployeeType == EmployeeType.Employee)
                employeeMapped.Rank = _salaryCalculator.CalculateRankForRegular(employeeMapped.Salary);

            else if (employeeMapped.EmployeeType == EmployeeType.Manager)
                employeeMapped.Rank = _salaryCalculator.CalculateRankForManager(employeeMapped.Salary);

            else
                employeeMapped.Rank = _salaryCalculator.CalculateRankForCEO(employeeMapped.Salary);

            return employeeMapped;
        }

        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            var employeesMapped = _mapper.Map<List<EmployeeDto>>(employees);
            var employeesWithManagedBy = AttachManagedByNameToEmployee(employeesMapped);
            var employeesGrouped = GroupEmployees(employeesWithManagedBy);

            return employeesGrouped;
        }

        public async Task<int> GetManagersId()
        {
            return await _employeeRepository.GetManagerId();
        }

        public async Task ClearManagerIdFromEmployees(int id)
        {
            await _employeeRepository.RemoveManagerIdFromEmployees(id);
        }

        public async Task<List<EmployeeDto>> GetNonRegularEmployees()
        {
            var nonRegular = await _employeeRepository.GetNonRegularEmployees();
            var nonRegularMapped = _mapper.Map<List<EmployeeDto>>(nonRegular);

            return nonRegularMapped;
        }

        public async Task CreateEmployee(EmployeeDto employeeDto, EmployeeType employeeType)
        {
            var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);

            if (employeeType == EmployeeType.Employee)
            {
                employee.IsManager = false;
                employee.IsCEO = false;
            }

            else if (employeeType == EmployeeType.Manager)
            {
                employee.IsManager = true;
            }

            else
            {
                if (await IsThereAnyExistingCeo())
                    throw new InvalidOperationException("OnlyOneCEOError");

                employee.IsCEO = true;
            }

            employee.Salary = MakeSalaryCalculation(employeeDto);
            await _employeeRepository.CreateEmployee(employee);
        }

        public async Task EditEmployee(EmployeeDto employeeDto)
        {
            var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);
            employee.Salary = MakeSalaryCalculation(employeeDto);
            await _employeeRepository.EditEmployee(employee);
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var employeeToDelete = await _employeeRepository.GetEmployee(id);
            return await _employeeRepository.DeleteEmployee(employeeToDelete);
        }

        public async Task<bool> IsThereAnyExistingCeo()
        {
            return await _employeeRepository.CheckIfCeoAlreadyExist();
        }

        public async Task<bool> IsManagingOther(int id)
        {
            return await _employeeRepository.CheckIfManagingOther(id);
        }

        private decimal MakeSalaryCalculation(EmployeeDto employeeDto)
        {
            if (employeeDto.IsManager)
                return _salaryCalculator.CalculateManagerSalary(employeeDto.Rank);

            else if (employeeDto.IsCEO)
                return _salaryCalculator.CalculateCEOSalary(employeeDto.Rank);

            return _salaryCalculator.CalculateRegularSalary(employeeDto.Rank);
        }

        // This method connects employees with their manager. I wanted to show "Managed By"
        // in the view and therefore I needed to loop over the employees. I was not a "criteria"
        // in the document but I thought it was a nice feature to see in the view.
        private List<EmployeeDto> AttachManagedByNameToEmployee(List<EmployeeDto> employees)
        {
            foreach (var employee in employees)
            {
                foreach (var nonRegularEmp in employees.Where(emp => emp.IsCEO || emp.IsManager))
                {
                    if (employee.ManagerId == nonRegularEmp.Id)
                    {
                        employee.ManagedBy = nonRegularEmp.FirstName + " " + nonRegularEmp.LastName;
                    }
                }
            }

            return employees;
        }

        // This one helps me order the employees by role.
        private List<EmployeeDto> GroupEmployees(List<EmployeeDto> employees)
        {
            var employeesGrouped = employees.OrderBy(emp => emp.EmployeeType).ToList();
            return employeesGrouped;
        }
    }
}