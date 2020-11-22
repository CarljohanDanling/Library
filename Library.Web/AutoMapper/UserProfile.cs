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

            CreateMap<LibraryItem, LibraryItemBase>()
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Type));

            CreateMap<DigitalMediaItem, LibraryItem>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.DigitalMediaItemType));

            CreateMap<NonDigitalMediaItem, LibraryItem>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.NonDigitalMediaItemType));

            CreateMap<LibraryItem, NonDigitalMediaItem>()
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Type));

            CreateMap<LibraryItem, DigitalMediaItem>()
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Type));

            CreateMap<LibraryItem, AudioBook>();

            CreateMap<LibraryItemBase, LibraryItem>();

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