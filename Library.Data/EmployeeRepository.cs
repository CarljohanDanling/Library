using Library.Data.Database.Context;
using Library.Data.Database.Models;
using Library.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Library.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly LibraryContext _libraryContext;

        public EmployeeRepository(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await _libraryContext.Employees.FirstOrDefaultAsync(emp => emp.Id == id);
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _libraryContext.Employees.ToListAsync();
        }

        public async Task<List<Employee>> GetNonRegularEmployees()
        {
            return await _libraryContext.Employees
                .Where(emp => emp.IsManager || emp.IsCEO).ToListAsync();
        }

        public async Task CreateEmployee(Employee employee)
        {
            await _libraryContext.AddAsync(employee);
            await _libraryContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteEmployee(Employee employee)
        {
            try
            {
                _libraryContext.Remove(employee);
                await _libraryContext.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                throw;
            }

            return true;
        }

        public async Task<bool> CheckIfCeoAlreadyExist()
        {
            return await _libraryContext.Employees.AnyAsync(emp => emp.IsCEO);
        }

        public async Task<bool> CheckIfManagingOther(int id)
        {
            return await _libraryContext.Employees.AnyAsync(emp => emp.ManagerId == id);
        }
    }
}