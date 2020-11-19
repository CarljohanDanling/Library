using AutoMapper;
using Library.Engine;
using Library.Web.Models;
using Library.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

            if (sortOption == "type")
            {
                viewModel.LibraryItems = viewModel.LibraryItems
                    .OrderBy(l => l.Type).ToList();
            }

            else
            {
                viewModel.LibraryItems = viewModel.LibraryItems
                    .OrderBy(l => l.Category.CategoryName).ToList();
            }

            return View(viewModel);
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
                //_libraryItemService.SparaNedItem

                return RedirectToAction("Index");
            }
            
            var categories = await _categoryService.GetCategories();
            var categoriesMapped = _mapper.Map<List<CategoryModel>>(categories);
            viewModel.Categories = categoriesMapped;
            viewModel.SelectedType = selectedType;

            return View(viewModel);
        }
    }
}