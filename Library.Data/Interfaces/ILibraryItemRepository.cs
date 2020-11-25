using Library.Data.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Data.Interfaces
{
    public interface ILibraryItemRepository
    {
        Task<List<LibraryItem>> GetLibraryItems();
        Task<LibraryItem> GetLibraryItem(int id);
        Task<bool> CreateLibraryItem(LibraryItem libraryItem);
        Task EditLibraryItem(LibraryItem libraryItem);
        Task CheckOutLibraryItem(LibraryItem libraryItem);
        Task CheckInLibraryItem(LibraryItem libraryItem);
        Task<bool> DeleteLibraryItem(LibraryItem libraryItem);
    }
}