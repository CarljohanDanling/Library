using Library.Data.Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Engine.ExtensionMethods;
using Library.Engine.Interface;
using Library.Data.Interfaces;

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

        // This might be ugly, but I set all library items that are not a reference book
        // to "IsBorrowable = true". This is because a reference book shouldn't be borrowable.
        public async Task<bool> CreateLibraryItem(LibraryItem libraryItem)
        {
            if (libraryItem.Type != "ReferenceBook")
            {
                libraryItem.IsBorrowable = true;
            }

            return await _libraryItemRepository.CreateLibraryItem(libraryItem);
        }

        public async Task LibraryItemOperations(LibraryItem libraryItem, string typeOfAction)
        {
            switch (typeOfAction)
            {
                case "Edit":
                    await EditLibraryItem(libraryItem);
                    break;
                case "Borrow":
                    await BorrowLibraryItem(libraryItem);
                    break;
                case "Return":
                    await ReturnLibraryItem(libraryItem);
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

        private async Task EditLibraryItem(LibraryItem libraryItem)
        {
            if (libraryItem.Type == "ReferenceBook")
            {
                libraryItem.BorrowDate = null;
                libraryItem.Borrower = null;
                libraryItem.IsBorrowable = false;
            }

            else if (libraryItem.IsBorrowable == false)
            {
                await _libraryItemRepository.EditLibraryItem(libraryItem);
            }

            else
            {
                libraryItem.IsBorrowable = true;
                await _libraryItemRepository.EditLibraryItem(libraryItem);
            }
        }

        private async Task BorrowLibraryItem(LibraryItem libraryItem)
        {
            libraryItem.IsBorrowable = false;
            libraryItem.Borrower = libraryItem.Borrower;
            libraryItem.BorrowDate = DateTime.Today;

            await _libraryItemRepository.BorrowLibraryItem(libraryItem);
        }

        private async Task ReturnLibraryItem(LibraryItem libraryItem)
        {
            libraryItem.IsBorrowable = true;
            libraryItem.Borrower = null;
            libraryItem.BorrowDate = null;

            await _libraryItemRepository.ReturnLibraryItem(libraryItem);
        }

        private List<LibraryItem> AddAcronymToTitle(List<LibraryItem> items)
        {
            var libraryItemsWithAcronymTitle = new List<LibraryItem>();

            foreach (var item in items)
            {
                item.Title = item.Title.ToAcronym();
                libraryItemsWithAcronymTitle.Add(item);
            }

            return libraryItemsWithAcronymTitle;
        }
    }
}
