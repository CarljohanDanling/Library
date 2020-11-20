using AutoMapper;
using Library.Data.Database.Models;
using Library.Web.Models;
using Library.Web.ViewModels;

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

            CreateMap<LibraryItem, AudioBookEdit>();
            CreateMap<LibraryItem, BookEdit>();
            CreateMap<LibraryItem, DvdEdit>();
            CreateMap<LibraryItem, ReferenceBookEdit>();

            CreateMap<AudioBookEdit, LibraryItem>();
            CreateMap<BookEdit, LibraryItem>();
            CreateMap<DvdEdit, LibraryItem>();
            CreateMap<ReferenceBookEdit, LibraryItem>();
        }
    }
}