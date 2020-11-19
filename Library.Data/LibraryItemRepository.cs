using AutoMapper;
using Library.Data.Database.Context;
using Library.Data.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Data
{
    public class LibraryItemRepository : ILibraryItemRepository
    {
        private readonly LibraryContext _libraryContext;
        private readonly IMapper _mapper;

        public LibraryItemRepository(LibraryContext libraryContext, IMapper mapper)
        {
            _libraryContext = libraryContext;
            _mapper = mapper;
        }

        public async Task<List<LibraryItem>> GetLibraryItems()
        {
            return await _libraryContext.LibraryItems.Include(c => c.Category).ToListAsync();
        }

        public async Task<LibraryItem> GetLibraryItem(int id)
        {
            var item = await _libraryContext.LibraryItems
                .Where(li => li.Id == id)
                .FirstOrDefaultAsync();

            return item;
        }

        public async Task<bool> CreateLibraryItem(LibraryItem libraryItem)
        {
            try
            {
                await _libraryContext.LibraryItems.AddAsync(libraryItem);
                await _libraryContext.SaveChangesAsync();

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}
