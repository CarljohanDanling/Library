using Library.Data.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Engine.Interface
{
    public interface ICategoryService
    {
        Task<Category> GetCategory(int id);
        Task<IEnumerable<Category>> GetCategories();
        Task<bool> CreateCategory(Category category);
        Task<bool> EditCategory(Category category);
        Task<bool> DeleteCategory(Category category);
    }
}