using Library.Engine.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Engine.Interface
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployee(int id);
        Task<List<EmployeeDto>> GetAllEmployees();
        Task CreateEmployee(EmployeeDto employeeDto);
        Task EditEmployee(EmployeeDto employee);
        Task<bool> DeleteEmployee(int id);
        Task<int> GetManagersId();
        Task<List<EmployeeDto>> GetNonRegularEmployees();
        Task ClearManagerIdFromEmployees(int id);
        Task<bool> IsThereAnyExistingCeo();
        Task<bool> IsManagingOther(int id);
    }
}