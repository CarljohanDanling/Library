using Library.Data;
using Library.Data.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Library.Engine.ExtensionMethods;


namespace Library.Engine
{
    public class LibraryItemService : ILibraryItemService
    {
        private readonly ILibraryItemRepository _libraryItemRepository;

        public LibraryItemService(ILibraryItemRepository libraryItemRepository)
        {
            _libraryItemRepository = libraryItemRepository;
        }

        public async Task<IEnumerable<LibraryItem>> GetLibraryItems()
        {
            var items = await _libraryItemRepository.GetLibraryItems();
            var itemsWithAcronymTitle = AddAcronymToTitle(items);

            return items;
        }

        private List<LibraryItem> AddAcronymToTitle(List<LibraryItem> items)
        {
            var libraryItemsWithAcronymTitle = new List<LibraryItem>();

            foreach (var item in items)
            {
                item.Title = item.Title.Acronym();
                libraryItemsWithAcronymTitle.Add(item);
            }

            return libraryItemsWithAcronymTitle;
        }
    }
}
