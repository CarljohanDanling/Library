using AutoMapper;
using Library.Data.Database.Models;
using Library.Web.Models;

namespace Library.Web.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();

            CreateMap<LibraryItem, LibraryItemModel>();

            CreateMap<AudioBookCreate, LibraryItem>();
            CreateMap<BookCreate, LibraryItem>();
            CreateMap<DvdCreate, LibraryItem>();
            CreateMap<ReferenceBookCreate, LibraryItem>();
        }
    }
}