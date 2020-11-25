using Library.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Library.Data.Database.Models;
using Library.Engine.Interface;

namespace Library.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategories();
            var categoriesMapped = _mapper.Map<List<CategoryModel>>(categories);

            return View(categoriesMapped);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            var createCategory = _mapper.Map<Category>(category);
            bool isDuplicateNameError;

            if (ModelState.IsValid)
            {
                isDuplicateNameError = await _categoryService.CreateCategory(createCategory);
                if (isDuplicateNameError)
                {
                    ModelState.AddModelError("DuplicateNameError", "A category with that name already exist.");
                    return View(category);
                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategory(id);
            var categoryMapped = _mapper.Map<CategoryModel>(category);

            return View(categoryMapped);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryModel category)
        {
            var editCategory = _mapper.Map<Category>(category);
            bool isDuplicateNameError;

            if (ModelState.IsValid)
            {
                isDuplicateNameError = await _categoryService.EditCategory(editCategory);
                if (isDuplicateNameError)
                {
                    ModelState.AddModelError("DuplicateNameError", "A category with that name already exist.");
                    return View(category);
                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategory(id);
            var categoryMapped = _mapper.Map<CategoryModel>(category);
                
            return View(categoryMapped);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryModel category)
        {
            var categoryMapped = _mapper.Map<Category>(category);

            var successfullyDeleted = await _categoryService.DeleteCategory(categoryMapped);

            if (successfullyDeleted)
            {
                return RedirectToAction("Index");
            }

            else
            {
                ModelState.AddModelError("CategoryReferenceError", "Cannot delete, category is still referenced");
                return View(category);
            }
        }
    }
}