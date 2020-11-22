using AutoMapper;
using Library.Data.Database.Models;
using Library.Engine;
using Library.Web.Models;
using Library.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Controllers
{
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

            var sortedItems = Sorter(viewModel, sortOption);

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

        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _categoryService.GetCategories();
            var libraryItem = await _libraryItemService.GetLibraryItem(id);
            var categoriesMapped = _mapper.Map<List<CategoryModel>>(categories);

            var viewModel = new EditLibraryItemViewModel
            {
                Categories = categoriesMapped
            };

            LibraryItemType libraryItemType;
            Enum.TryParse(libraryItem.Type, out libraryItemType);

            switch (libraryItemType)
            {
                case LibraryItemType.AudioBook:
                case LibraryItemType.Dvd:
                    var digitalMediaItem = _mapper.Map<DigitalMediaItem>(libraryItem);
                    viewModel.DigitalMediaItem = digitalMediaItem;
                    return View(viewModel);

                case LibraryItemType.Book:
                case LibraryItemType.ReferenceBook:
                    var nonDigitalMediaItem = _mapper.Map<NonDigitalMediaItem>(libraryItem);
                    viewModel.NonDigitalMediaItem = nonDigitalMediaItem;
                    return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditLibraryItemViewModel viewModel, MediaItemCategory mediaItemCategory, string submit)
        {
            var itemType = viewModel.LibraryItemType;

            if (ModelState.IsValid)
            {
                switch (mediaItemCategory)
                {
                    case MediaItemCategory.DigitalMedia:
                        var digitalItem = _mapper.Map<LibraryItem>(viewModel.DigitalMediaItem);
                        await _libraryItemService.EditLibraryItem(digitalItem, submit);
                        break;
                    case MediaItemCategory.NonDigitalMedia:
                        var nonDigitalItem = _mapper.Map<LibraryItem>(viewModel.NonDigitalMediaItem);
                        await _libraryItemService.EditLibraryItem(nonDigitalItem, submit);
                        break;
                    default:
                        break;
                }

                return RedirectToAction("Index");
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

        private LibraryItemViewModel Sorter(LibraryItemViewModel viewModel, string sortOption)
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