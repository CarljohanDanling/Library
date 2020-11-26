using AutoMapper;
using Library.Data.Database.Models;
using Library.Engine.Dtos;
using Library.Engine.Enums;
using Library.Web.Models;

namespace Library.Web.AutoMapper
{
    public class UserProfile : Profile
    {
        // This is where I make mappings of different classes.
        // I also have some logic built into the mapping, like the
        // one where I map the bool representation of IsCEO and IsManager from
        // the database into my EmployeeType enum. Very handy!
        public UserProfile()
        {
            // Category
            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();

            // Library
            CreateMap<LibraryItem, LibraryItemBase>()
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Type));

            CreateMap<LibraryItemBase, LibraryItem>();

            CreateMap<DigitalMediaItem, LibraryItem>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.DigitalMediaItemType));

            CreateMap<NonDigitalMediaItem, LibraryItem>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.NonDigitalMediaItemType));

            CreateMap<LibraryItem, NonDigitalMediaItem>()
                .ForMember(dest => dest.NonDigitalMediaItemType, opt => opt.MapFrom(src => src.Type));

            CreateMap<LibraryItem, DigitalMediaItem>()
                .ForMember(dest => dest.DigitalMediaItemType, opt => opt.MapFrom(src => src.Type));

            // Employee
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.EmployeeType, opt => opt.MapFrom((src, dest) =>
                {
                    var isManager = src.IsManager;
                    var isCEO = src.IsCEO;

                    if (!isManager && !isCEO) return EmployeeType.Employee;
                    if (isManager) return EmployeeType.Manager;
                    return EmployeeType.CEO;
                }));

            CreateMap<EmployeeDto, Employee>();

            CreateMap<EmployeeDto, EmployeeModel>()
                .ForMember(dest => dest.Name, opt => opt
                    .MapFrom(src => string.Format("{0} {1}", src.FirstName, src.LastName)));

            CreateMap<EmployeeDto, NonRegularEmployeeModel>()
                .ForMember(dest => dest.Name, opt => opt
                    .MapFrom(src => string.Format("{0} {1}", src.FirstName, src.LastName)));

            CreateMap<EmployeeModel, EmployeeDto>();
        }
    }
}