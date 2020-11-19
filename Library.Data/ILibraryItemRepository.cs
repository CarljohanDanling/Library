using Library.Data.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    public interface ILibraryItemRepository
    {
        Task<List<LibraryItem>> GetLibraryItems();
    }
}