using Library.Data.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task<bool> CreateCategory(Category category);
        Task<bool> EditCategory(Category category);
        Task DeleteCategory(Category category);
    }
}