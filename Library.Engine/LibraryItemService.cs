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

            return itemsWithAcronymTitle;
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

        // This methods differenties between what kind of library item it is. It also performs some logic from rules that I decided.
        // There is ONE holy rule that applies to all library items; IF an item is borrowed, you cannot edit anything before RETURNING it.
        // The ReferenceBook is a bit special, hence the more logic in the Else {} code block.
        private async Task<bool> EditHandler(LibraryItem libraryItem, string fromType)
        {
            if (libraryItem.Type == "AudioBook" || libraryItem.Type == "Dvd")
            {
                if (libraryItem.IsBorrowable)
                {
                    await EditLibraryItem(libraryItem);
                    return true;
                }

                throw new InvalidOperationException();
            }

            else
            {
                if (fromType == "Book" && libraryItem.IsBorrowable)
                {
                    libraryItem.IsBorrowable = false;
                    await EditLibraryItem(libraryItem);
                    return true;
                }

                else if (fromType == "ReferenceBook")
                {
                    libraryItem.IsBorrowable = true;
                    await EditLibraryItem(libraryItem);
                    return true;
                }

                throw new InvalidOperationException();
            }
        }

        // This method checks what action the user is taking, then guiding the code to the right action ("Edit", "Borrow" and "Return").
        public async Task<bool> MediaCoordinator(string typeOfAction, LibraryItem libraryItem, string fromType)
        {
            if (typeOfAction == "Edit")
            {
                try {await EditHandler(libraryItem, fromType);}

                catch (Exception) {throw new InvalidOperationException("NonBorrowableError");}

                return true;
            }

            else if (typeOfAction == "Borrow")
            {
                if (libraryItem.Borrower == null)
                    throw new InvalidOperationException("BorrowerNameIsNullError");

                await BorrowLibraryItem(libraryItem);
                return true;
            }

            await ReturnLibraryItem(libraryItem);
            return true;
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

                await _libraryItemRepository.EditLibraryItem(libraryItem);
            }

            else if (libraryItem.IsBorrowable == false)
                await _libraryItemRepository.EditLibraryItem(libraryItem);

            else
                libraryItem.IsBorrowable = true;
                await _libraryItemRepository.EditLibraryItem(libraryItem);
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