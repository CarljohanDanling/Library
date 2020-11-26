using Library.Data.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployee(int id);
        Task<List<Employee>> GetAllEmployees();
        Task<List<Employee>> GetNonRegularEmployees();
        Task<int> GetManagerId();
        Task RemoveManagerIdFromEmployees(int id);
        Task CreateEmployee(Employee employee);
        Task EditEmployee(Employee employee);
        Task<bool> DeleteEmployee(Employee employee);
        Task<bool> CheckIfCeoAlreadyExist();
        Task<bool> CheckIfManagingOther(int id);
    }
}