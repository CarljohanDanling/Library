using Library.Data.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Engine.Interface
{
    public interface ILibraryItemService
    {
        Task<IEnumerable<LibraryItem>> GetLibraryItems();
        Task<LibraryItem> GetLibraryItem(int id);
        Task<bool> CreateLibraryItem(LibraryItem libraryItem);
        Task EditLibraryItem(LibraryItem libraryItem, string submit);
        Task<bool> DeleteLibraryItem(int id);
    }
}