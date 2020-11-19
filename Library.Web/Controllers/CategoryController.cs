using Library.Data.Database.Context;
using Library.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Engine;
using Library.Engine.Dto;

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
            var createCategoryDto = _mapper.Map<CategoryDto>(category);
            bool isDuplicateNameError;

            if (ModelState.IsValid)
            {
                isDuplicateNameError = await _categoryService.CreateCategory(createCategoryDto);
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
            var editCategoryDto = _mapper.Map<CategoryDto>(category);
            bool isDuplicateNameError;

            if (ModelState.IsValid)
            {
                isDuplicateNameError = await _categoryService.EditCategory(editCategoryDto);
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
            var categoryMapped = _mapper.Map<CategoryDto>(category);

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