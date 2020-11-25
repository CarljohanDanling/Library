using AutoMapper;
using Library.Data.Database.Models;
using Library.Data.Interfaces;
using Library.Engine.Dtos;
using Library.Engine.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            var employeesMapped = _mapper.Map<List<EmployeeDto>>(employees);

            foreach (var employee in employeesMapped)
            {
                foreach (var nonRegularEmp in employeesMapped.Where(emp => emp.IsCEO || emp.IsManager))
                {
                    if (employee.ManagerId == nonRegularEmp.Id)
                    {
                        employee.ManagedBy = nonRegularEmp.FirstName + " " + nonRegularEmp.LastName;
                    }
                }
            }
            
            var employeesGrouped = GroupEmployees(employeesMapped);

            return employeesGrouped;
        }

        public async Task<List<EmployeeDto>> GetNonRegularEmployees()
        {
            var managers = await _employeeRepository.GetNonRegularEmployees();
            var managersMapped = _mapper.Map<List<EmployeeDto>>(managers);

            return managersMapped;
        }

        public async Task CreateEmployee(EmployeeDto employeeDto)
        {
            var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);

            if (employeeDto.IsManager)
            {
                employee.Salary = _salaryCalculator.ManagerSalary(employeeDto.Rank);
            }

            else if (employeeDto.IsCEO)
            {
                employee.Salary = _salaryCalculator.CEOSalary(employeeDto.Rank);
            }

            else
            {
                employee.Salary = _salaryCalculator.RegularSalary(employeeDto.Rank);
            }

            await _employeeRepository.CreateEmployee(employee);
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

        private List<EmployeeDto> GroupEmployees(List<EmployeeDto> employees)
        {
            var employeesGrouped = employees.OrderBy(emp => emp.EmployeeType.ToString()).ToList();
            return employeesGrouped;
        }
    }
}