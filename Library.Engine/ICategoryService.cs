using Library.Data.Database.Models;
using Library.Engine.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Engine
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task<bool> CreateCategory(CategoryDto category);
        Task<bool> EditCategory(CategoryDto categoryDto);
        Task<bool> DeleteCategory(CategoryDto categoryDto);
    }
}