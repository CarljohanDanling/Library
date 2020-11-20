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

        public async Task<LibraryItem> GetLibraryItem(int id)
        {
            return await _libraryItemRepository.GetLibraryItem(id);
        }

        public async Task<bool> CreateLibraryItem(LibraryItem libraryItem)
        {
            if (libraryItem.Type != "ReferenceBook")
            {
                libraryItem.IsBorrowable = true;
            }

            return await _libraryItemRepository.CreateLibraryItem(libraryItem);
        }

        public async Task EditLibraryItem(LibraryItem libraryItem, string submit)
        {
            switch (submit)
            {
                case "Edit":
                    await _libraryItemRepository.EditLibraryItem(libraryItem);
                    break;
                case "CheckOut":
                    await CheckOutLibraryItem(libraryItem);
                    break;
                case "CheckIn":
                    await CheckInLibraryItem(libraryItem);
                    break;
                default: 
                    throw new Exception("No submit action chosen");
            }
        }

        private async Task CheckOutLibraryItem(LibraryItem libraryItem)
        {
            var item = await _libraryItemRepository.GetLibraryItem(libraryItem.Id);

            item.IsBorrowable = false;
            item.Borrower = libraryItem.Borrower;
            item.BorrowDate = DateTime.Today;

            await _libraryItemRepository.CheckOutLibraryItem(item);
        }

        private async Task CheckInLibraryItem(LibraryItem libraryItem)
        {
            var item = await _libraryItemRepository.GetLibraryItem(libraryItem.Id);

            item.IsBorrowable = true;
            item.Borrower = null;
            item.BorrowDate = null;

            await _libraryItemRepository.CheckInLibraryItem(item);
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
