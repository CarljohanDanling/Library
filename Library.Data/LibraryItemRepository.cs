using AutoMapper;
using Library.Data.Database.Context;
using Library.Data.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
