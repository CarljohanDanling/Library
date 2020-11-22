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
            
            CreateMap<LibraryItemBase, LibraryItem>();

            CreateMap<DigitalMediaItem, LibraryItem>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.DigitalMediaItemType));

            CreateMap<NonDigitalMediaItem, LibraryItem>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.NonDigitalMediaItemType));

            CreateMap<LibraryItem, NonDigitalMediaItem>()
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Type));

            CreateMap<LibraryItem, DigitalMediaItem>()
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Type));
        }
    }
}