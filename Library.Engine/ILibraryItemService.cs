using Library.Data.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Engine
{
    public interface ILibraryItemService
    {
        Task<IEnumerable<LibraryItem>> GetLibraryItems();
    }
}
