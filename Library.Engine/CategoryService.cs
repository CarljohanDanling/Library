using AutoMapper;
using Library.Data;
using Library.Data.Database.Models;
using Library.Data.Models;
using Library.Engine.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Library.Engine
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILibraryItemRepository _libraryItemRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, ILibraryItemRepository libraryItemRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _libraryItemRepository = libraryItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categoryRepository.GetCategories();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _categoryRepository.GetCategory(id);
        }

        public async Task<bool> CreateCategory(CategoryDto category)
        {
            var createCategoryModel = _mapper.Map<CategoryModelDL>(category);

            return await _categoryRepository.CreateCategory(createCategoryModel);
        }

        public async Task<bool> EditCategory(CategoryDto category)
        {
            var editCategoryModel = _mapper.Map<CategoryModelDL>(category);

            return await _categoryRepository.EditCategory(editCategoryModel);
        }

        public async Task<bool> DeleteCategory(CategoryDto categoryDto)
        {
            var libraryItems = await _libraryItemRepository.GetLibraryItems();

            var anyReferencedCategory = libraryItems.Any(li => li.CategoryId == categoryDto.Id);

            if (anyReferencedCategory)
            {
                return false;
            }

            var category = _mapper.Map<CategoryModelDL>(categoryDto);
            await _categoryRepository.DeleteCategory(category);

            return true;
        }
    }
}
