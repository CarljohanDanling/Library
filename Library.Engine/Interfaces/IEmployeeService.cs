using Library.Engine.Dtos;
using Library.Engine.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Engine.Interface
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployee(int id);
        Task<List<EmployeeDto>> GetAllEmployees();
        Task EmployeeCoordinator(EmployeeDto employeeDto, EmployeeType currentEmployeeType,
            EmployeeType newEmployeeType);
        Task CreateEmployee(EmployeeDto employeeDto, EmployeeType employeeType);
        Task EditEmployee(EmployeeDto employee);
        Task<bool> DeleteEmployee(int id);
        Task<int> GetOneManagerId();
        Task<List<EmployeeDto>> GetNonRegularEmployees();
        Task ClearManagerIdFromEmployees(int id);
        Task<bool> IsManagingOther(int id);
    }
}