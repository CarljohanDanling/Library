using Library.Data.Database.Context;
using Library.Data.Database.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Library.Data.Interfaces;

namespace Library.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LibraryContext _libraryContext;
        private readonly IMapper _mapper;

        public CategoryRepository(LibraryContext libraryContext, IMapper mapper)
        {
            _libraryContext = libraryContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _libraryContext.Categories.ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _libraryContext.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<bool> CreateCategory(Category category)
        {
            try
            {
                await _libraryContext.Categories.AddAsync(category);
                await _libraryContext.SaveChangesAsync();
            }

            catch (DbUpdateException ex)
            {
                var sqlException = ex.InnerException as SqlException;
                var isDuplicateName = sqlException.Number == 2601;

                if (isDuplicateName)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> EditCategory(Category category)
        {
            try
            {
                var successfullyUpdate = _libraryContext.Update(category);
                await _libraryContext.SaveChangesAsync();
            }

            catch (SqlException ex)
            {
                var isDuplicateName = ex.Number == 2601;

                if (isDuplicateName)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task DeleteCategory(Category category)
        {
            _libraryContext.Categories.Remove(category);
            await _libraryContext.SaveChangesAsync();
        }
    }
}