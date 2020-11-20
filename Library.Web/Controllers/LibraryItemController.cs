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
            var libraryItemsMapped = _mapper.Map<List<LibraryItemModel>>(libraryItems);

            var viewModel = new LibraryItemViewModel
            {
                LibraryItems = libraryItemsMapped,
                LibraryItemModel = new LibraryItemModel()
            };

            var sortedItems = Sorter(viewModel, sortOption);

            return View(sortedItems);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var libraryItem = await _libraryItemService.GetLibraryItem(id);
            var categories = await _categoryService.GetCategories();

            var categoriesMapped = _mapper.Map<List<CategoryModel>>(categories);
            var viewModel = new EditLibraryItemViewModel
            {
                Categories = categoriesMapped
            };

            switch (libraryItem.Type)
            {
                case "AudioBook":
                    var audioBook = _mapper.Map<AudioBookEdit>(libraryItem);
                    audioBook.Categories = categoriesMapped;
                    viewModel.AudioBook = audioBook;
                    return View(viewModel);
                case "Book":
                    var book = _mapper.Map<BookEdit>(libraryItem);
                    book.Categories = categoriesMapped;
                    viewModel.Book = book;
                    return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditLibraryItemViewModel viewModel, string submit, string type)
        {
            if (ModelState.IsValid)
            {
                switch (type)
                {
                    case "AudioBook":
                        var audioBook = _mapper.Map<LibraryItem>(viewModel.AudioBook);
                        await _libraryItemService.EditLibraryItem(audioBook, submit);
                        break;
                    case "Book":
                        var book = _mapper.Map<LibraryItem>(viewModel.Book);
                        await _libraryItemService.EditLibraryItem(book, submit);
                        break;
                }
            }

            return RedirectToAction("Index");
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
        public async Task<IActionResult> Create(CreateLibraryItemViewModel viewModel, string selectedType)
        {
            if (ModelState.IsValid)
            {
                switch (selectedType)
                {
                    case "audioBook":
                        var audioBook = _mapper.Map<LibraryItem>(viewModel.AudioBook);
                        await _libraryItemService.CreateLibraryItem(audioBook);
                        break;
                    case "book":
                        var book = _mapper.Map<LibraryItem>(viewModel.Book);
                        await _libraryItemService.CreateLibraryItem(book);
                        break;
                    case "dvd":
                        var dvd = _mapper.Map<LibraryItem>(viewModel.Dvd);
                        await _libraryItemService.CreateLibraryItem(dvd);
                        break;
                    case "referenceBook":
                        var referenceBook = _mapper.Map<LibraryItem>(viewModel.ReferenceBook);
                        await _libraryItemService.CreateLibraryItem(referenceBook);
                        break;
                }

                return RedirectToAction("Index");
            }

            viewModel.SelectedType = selectedType;
            return View(viewModel);
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
                        .OrderBy(l => l.Type).ToList();
                }
            }

            return viewModel;
        }
    }
}