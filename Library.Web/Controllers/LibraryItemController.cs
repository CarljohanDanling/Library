using AutoMapper;
using Library.Data.Database.Models;
using Library.Web.Models;
using Library.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Engine.Interface;
using System;

namespace Library.Web.Controllers
{
    // The library part was the one I struggled most with. When I started working on it I chose to
    // have a model for each library item. I also had one view for "creating" and one for "editing"
    // each library item. For example "CreateAudioBook.cshtml" and "EditAudioBook.cshtml". This seemed like
    // a good move at first, but, when I started to think about it, I found that having it like that
    // would make the application very complex if scaled up. What if the library had 1000 items. Would I create 1000 
    // models and all corresponding views? NO!

    // I somehow struggled to have a pure generic model, like "LibraryItem". I came to the conclusion to group the 4 different
    // library items into 2 categories; DigitalMedia and NonDigitalMedia. AudioBook and DVD shares the same input fields in the views
    // and so do ReferenceBook and Book. Therefore I thought it was a good move to do that.

    // I ended up with having one view for creating and one for editing, for each category. This is not optimal, because if the system
    // were to have more items, the probability needing more different input fields would make the administration hard.

    public class LibraryItemController : Controller
    {
        private readonly ILibraryItemService _libraryItemService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public LibraryItemController(ILibraryItemService libraryItemService, ICategoryService categoryService, IMapper mapper)
        {
            _libraryItemService = libraryItemService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string sortOption)
        {
            var libraryItems = await _libraryItemService.GetLibraryItems();
            var libraryItemsMapped = _mapper.Map<List<LibraryItemBase>>(libraryItems);

            var viewModel = new LibraryItemViewModel
            {
                LibraryItems = libraryItemsMapped,
                LibraryItemBase = new LibraryItemBase()
            };

            var sortedItems = SortHandler(viewModel, sortOption);

            return View(sortedItems);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategories();
            var categoriesMapped = _mapper.Map<List<CategoryModel>>(categories);

            var viewModel = new CreateLibraryItemViewModel
            {
                Categories = categoriesMapped
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLibraryItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                switch (viewModel.MediaItemCategory)
                {
                    case MediaItemCategory.DigitalMedia:
                        var digitalMedia = _mapper.Map<LibraryItem>(viewModel.DigitalMedia);
                        await _libraryItemService.CreateLibraryItem(digitalMedia);
                        break;
                    case MediaItemCategory.NonDigitalMedia:
                        var nonDigitalMedia = _mapper.Map<LibraryItem>(viewModel.NonDigitalMedia);
                        await _libraryItemService.CreateLibraryItem(nonDigitalMedia);
                        break;
                }

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // This method gets the item and all categories from database.
        // It constructs up a viewmodel which the view uses.
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _categoryService.GetCategories();
            var libraryItem = await _libraryItemService.GetLibraryItem(id);
            var libraryItemMapped = _mapper.Map<LibraryItemBase>(libraryItem);
            var categoriesMapped = _mapper.Map<List<CategoryModel>>(categories);

            var viewModel = new EditLibraryItemViewModel
            {
                Categories = categoriesMapped
            };

            if (libraryItemMapped.ItemType == LibraryItemType.AudioBook || libraryItemMapped.ItemType == LibraryItemType.Dvd)
            {
                var digitalMediaItem = _mapper.Map<DigitalMediaItem>(libraryItemMapped);
                viewModel.DigitalMediaItem = digitalMediaItem;
                return View(viewModel);
            }

            var nonDigitalMediaItem = _mapper.Map<NonDigitalMediaItem>(libraryItemMapped);
            viewModel.NonDigitalMediaItem = nonDigitalMediaItem;
            return View(viewModel);
        }

        // This method calls MediaCoordinator (service layer). If an invalid operation takes place
        // in the service layer, it bubbles up to this method and applies model state error.
        [HttpPost]
        public async Task<IActionResult> Edit(EditLibraryItemViewModel viewModel, MediaItemCategory mediaItemCategory,
            string typeOfAction, LibraryItemType type)
        {
            var fromType = type.ToString();

            if (ModelState.IsValid)
            {
                if (mediaItemCategory == MediaItemCategory.DigitalMedia)
                {
                    var digitalItem = _mapper.Map<LibraryItem>(viewModel.DigitalMediaItem);

                    try
                    {
                        await _libraryItemService.MediaCoordinator(typeOfAction, digitalItem, fromType);
                    }

                    catch (InvalidOperationException ex)
                    {
                        GenerateModelError(ex);
                        return View(viewModel);
                    }

                    return RedirectToAction("Index", "LibraryItem");
                }

                else
                {
                    var nonDigitalItem = _mapper.Map<LibraryItem>(viewModel.NonDigitalMediaItem);

                    try
                    {
                        var nonDigitalSuccess = await _libraryItemService.MediaCoordinator(typeOfAction, nonDigitalItem, fromType);
                    }

                    catch (InvalidOperationException ex)
                    {
                        GenerateModelError(ex);
                        return View(viewModel);
                    }

                    return RedirectToAction("Index", "LibraryItem");
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _libraryItemService.GetLibraryItem(id);
            var itemMapped = _mapper.Map<LibraryItem, LibraryItemBase>(item);

            return View(itemMapped);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(LibraryItemBase libraryItem)
        {
            if (libraryItem.IsBorrowable || libraryItem.ItemType == LibraryItemType.ReferenceBook)
            {
                await _libraryItemService.DeleteLibraryItem(libraryItem.Id);
                return RedirectToAction("Index");
            }

            else
            {
                ModelState.AddModelError("NonBorrowableError", "The item is borrowed, please return it before deletion.");
                return View(libraryItem);
            }
        }

        private void GenerateModelError(InvalidOperationException ex)
        {
            if (ex.Message == "NonBorrowableError")
            {
                ModelState.AddModelError("NonBorrowableError", "The item is borrowed, return it before editing it.");
                return;
            }

            if (ex.Message == "BorrowerNameIsNullError")
            {
                ModelState.AddModelError("BorrowerError", "Must enter borrower");
                return;
            }
        }

        // This method solves the criteria: "Listing library items should be sorted by Category Name. This can be changed to
        // Type by the user. (This change need to persist in current session but not after application restart).
        // I struggled a bit with this I remember. I'm not very used to Sessions and before I figured out
        // how it worked I had a hard time figuring out how to save the user option through out the applications
        // life cycle. The solution is not elegant but not ugly either >__< .
        private LibraryItemViewModel SortHandler(LibraryItemViewModel viewModel, string sortOption)
        {
            var sessionSorting = HttpContext.Session.GetString("Sorting");

            if (sortOption == null && sessionSorting == null)
            {
                viewModel.LibraryItems = viewModel.LibraryItems
                    .OrderBy(l => l.Category.CategoryName).ToList();
            }

            else
            {
                if (sortOption == "category" || sessionSorting == "category" && sortOption == null)
                {
                    HttpContext.Session.SetString("Sorting", "category");
                    viewModel.LibraryItems = viewModel.LibraryItems
                        .OrderBy(l => l.Category.CategoryName).ToList();
                }

                else
                {
                    HttpContext.Session.SetString("Sorting", "type");
                    viewModel.LibraryItems = viewModel.LibraryItems
                        .OrderBy(l => l.ItemType).ToList();
                }
            }

            return viewModel;
        }
    }
}