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
    // This repository handles actions related to employees

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

        public async Task<int> GetOneManagerId()
        {
            return await _libraryContext.Employees
                .Where(emp => emp.IsManager)
                .Select(emp => emp.Id)
                .FirstAsync();
        }

        // This is used to get all managers and, if present, CEO.
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

        // This method helps me to clear the ManagerId when an Manager advances
        // to CEO. Because the manager manages regular employees and being an
        // CEO you can't manage regular employees.
        public async Task RemoveManagerIdFromEmployees(int id)
        {
            await _libraryContext.Employees
                .Where(emp => emp.ManagerId == id)
                .ForEachAsync(emp => emp.ManagerId = null);

            await _libraryContext.SaveChangesAsync();
        }

        public async Task EditEmployee(Employee employee)
        {
            _libraryContext.Update(employee);
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