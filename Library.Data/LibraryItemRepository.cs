using AutoMapper;
using Library.Data.Database.Context;
using Library.Data.Database.Models;
using System;
using System.Collections.Generic;
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
                .Include(l => l.Category)
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
            }
            
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task EditLibraryItem(LibraryItem libraryItem)
        {
            _libraryContext.Update(libraryItem);
            await _libraryContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteLibraryItem(LibraryItem libraryItem)
        {
            try
            {
                _libraryContext.Remove(libraryItem);
                await _libraryContext.SaveChangesAsync();
            }

            catch (InvalidOperationException)
            {
                throw;
            }

            return true;
        }

        public async Task CheckOutLibraryItem(LibraryItem libraryItem)
        {
            _libraryContext.LibraryItems.Update(libraryItem);
            await _libraryContext.SaveChangesAsync();
        }

        public async Task CheckInLibraryItem(LibraryItem libraryItem)
        {
            _libraryContext.LibraryItems.Update(libraryItem);
            await _libraryContext.SaveChangesAsync();
        }
    }
}