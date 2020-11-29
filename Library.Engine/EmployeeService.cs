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

        // Im not very happy with this method. First, its too big and secondly it screams "DRY".
        // The thing is, I need to take different actions depending on the empoloyee type and user action. 
        // I found it hard to make a generic version. The "isChangingEmployeeType" is used to check if the employee
        // wants to make a shift from one employee type to another.
        public async Task EmployeeCoordinator(EmployeeDto employeeDto, EmployeeType currentEmployeeType,
            EmployeeType newEmployeeType)
        {
            var isChangingEmployeeType = currentEmployeeType != newEmployeeType;

            if (isChangingEmployeeType)
            {
                switch (currentEmployeeType)
                {
                    case EmployeeType.Employee:
                        await RegularEmployeeHandler(employeeDto, currentEmployeeType, newEmployeeType);
                        break;
                    case EmployeeType.Manager:
                        await ManagerHandler(employeeDto, currentEmployeeType, newEmployeeType);
                        break;
                    case EmployeeType.CEO:
                        await CEOHandler(employeeDto, currentEmployeeType, newEmployeeType);
                        break;
                }

                return;
            }

            switch (currentEmployeeType)
            {
                case EmployeeType.Employee:
                    await EditEmployee(employeeDto);
                    break;
                case EmployeeType.Manager:
                    employeeDto.IsManager = true;
                    await EditEmployee(employeeDto);
                    break;
                case EmployeeType.CEO:
                    employeeDto.IsCEO = true;
                    await EditEmployee(employeeDto);
                    break;
            }
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

        // Getting all employees together with the name of their manager
        // and also grouping them by employee type.
        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            var employeesMapped = _mapper.Map<List<EmployeeDto>>(employees);
            var employeesWithManagedBy = AttachManagedByNameToEmployee(employeesMapped);
            var employeesGrouped = GroupEmployees(employeesWithManagedBy);

            return employeesGrouped;
        }

        public async Task<int> GetOneManagerId()
        {
            return await _employeeRepository.GetOneManagerId();
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

            employee.Salary = MakeSalaryCalculation(employee, employeeDto.Rank);
            await _employeeRepository.CreateEmployee(employee);
        }

        public async Task EditEmployee(EmployeeDto employeeDto)
        {
            var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);
            employee.Salary = MakeSalaryCalculation(employee, employeeDto.Rank);
            await _employeeRepository.EditEmployee(employee);
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var employeeToDelete = await _employeeRepository.GetEmployee(id);
            return await _employeeRepository.DeleteEmployee(employeeToDelete);
        }

        public async Task<bool> IsManagingOther(int id)
        {
            return await _employeeRepository.CheckIfManagingOther(id);
        }

        // This is the handler for the regular employee. I differentiate what the users wants to do by checking
        // the current employee type with what the user chose. I think the variable name "regularEmployeeToManager" is quite
        // self explanatory. I chose to make the regular employee able to upgrade to manager or ceo.
        private async Task RegularEmployeeHandler(EmployeeDto employeeDto, EmployeeType currentEmployeeType, 
            EmployeeType newEmployeeType)
        {
            var regularEmployeeToManager = 
                currentEmployeeType == EmployeeType.Employee && newEmployeeType == EmployeeType.Manager;

            // Regular Employee -> Manager
            if (regularEmployeeToManager)
            {
                employeeDto.IsManager = true;
                await EditEmployee(employeeDto);
                return;
            }

            // Regular Employee -> CEO
            // If employee wants to be a CEO, I need to check if there already exist one.
            if (await IsThereAnyExistingCeo())
                throw new InvalidOperationException("OnlyOneCEOError");

            // Removes ManagerId because CEO cannot be managed by anyone.
            employeeDto.ManagerId = null;
            employeeDto.IsCEO = true;
            await EditEmployee(employeeDto);
        }

        // This is the handler for the managers. As in the previous method is differentiate the user's action and
        // try to catch errors. I took the decision that a Manager cannot downgrade to a regular employee.
        private async Task ManagerHandler(EmployeeDto employeeDto, EmployeeType currentEmployeeType, 
            EmployeeType newEmployeeType)
        {
            var managerToEmployee = 
                currentEmployeeType == EmployeeType.Manager && newEmployeeType == EmployeeType.Employee;

            // Manager -> Regular Employee
            if (managerToEmployee)
            {
                throw new InvalidOperationException("ManagerToRegularEmployeeError");
            }

            // Manager -> CEO, checking if its possible
            if (await IsThereAnyExistingCeo())
            {
                throw new InvalidOperationException("OnlyOneCEOError");
            }

            // Manager -> CEO success
            await ClearManagerIdFromEmployees(employeeDto.Id);
            employeeDto.IsCEO = true;
            employeeDto.ManagerId = null;
            await EditEmployee(employeeDto);
        }

        // This is the handler for the CEO. I added support for the CEO to downgrade
        // to manager or even regular employee.
        private async Task CEOHandler(EmployeeDto employee, EmployeeType currentType, EmployeeType newType)
        {
            var ceoToManager = 
                currentType == EmployeeType.CEO && newType == EmployeeType.Manager;

            // CEO -> MAager
            if (ceoToManager)
            {
                employee.IsManager = true;
                await EditEmployee(employee);
                return;
            }

            // CEO -> Regular Employee
            await ClearManagerIdFromEmployees(employee.Id);
            
            // Assigns an manager, because as a regular employee, you need someone to report to.
            var managerId = await GetOneManagerId();
            employee.ManagerId = managerId;

            await EditEmployee(employee);
            return;
        }

        private async Task<bool> IsThereAnyExistingCeo()
        {
            return await _employeeRepository.CheckIfCeoAlreadyExist();
        }

        private decimal MakeSalaryCalculation(Employee employee, int rank)
        {
            if (employee.IsManager)
                return _salaryCalculator.CalculateManagerSalary(rank);

            else if (employee.IsCEO)
                return _salaryCalculator.CalculateCEOSalary(rank);

            return _salaryCalculator.CalculateRegularSalary(rank);
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