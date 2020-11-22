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
                case "Check Out":
                    await CheckOutLibraryItem(libraryItem);
                    break;
                case "Check In":
                    await CheckInLibraryItem(libraryItem);
                    break;
                default:
                    throw new Exception("No submit action chosen");
            }
        }

        public async Task<bool> DeleteLibraryItem(int id)
        {
            var itemToDelete = await _libraryItemRepository.GetLibraryItem(id);

            try
            {
                var ableToDelete = await _libraryItemRepository.DeleteLibraryItem(itemToDelete);
                return ableToDelete;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        private async Task CheckOutLibraryItem(LibraryItem libraryItem)
        {
            libraryItem.IsBorrowable = false;
            libraryItem.Borrower = libraryItem.Borrower;
            libraryItem.BorrowDate = DateTime.Today;

            await _libraryItemRepository.CheckOutLibraryItem(libraryItem);
        }

        private async Task CheckInLibraryItem(LibraryItem libraryItem)
        {
            libraryItem.IsBorrowable = true;
            libraryItem.Borrower = null;
            libraryItem.BorrowDate = null;

            await _libraryItemRepository.CheckInLibraryItem(libraryItem);
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
