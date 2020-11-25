using Library.Data.Database.Models;
using Library.Engine.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Engine.Interface
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployee(int id);
        Task<List<EmployeeDto>> GetAllEmployees();
        Task<List<EmployeeDto>> GetNonRegularEmployees();
        Task CreateEmployee(EmployeeDto employeeDto);
        Task<bool> DeleteEmployee(int id);
        Task<bool> IsThereAnyExistingCeo();
        Task<bool> IsManagingOther(int id);
    }
}