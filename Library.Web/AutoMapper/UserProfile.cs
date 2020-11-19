using AutoMapper;
using Library.Data.Database.Models;
using Library.Data.Models;
using Library.Engine.Dto;
using Library.Web.Models;

namespace Library.Web.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CategoryDto, CategoryModel>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CategoryDto, CategoryModelDL>();
            
            CreateMap<CategoryModel, Category>();
            CreateMap<CategoryModel, CategoryDto>();

            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryModelDL, Category>();

            CreateMap<LibraryItem, LibraryItemModel>();
        }
    }
}