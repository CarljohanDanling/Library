using AutoMapper;
using Library.Data.Database.Models;
using Library.Engine.Dtos;
using Library.Engine.Enums;
using Library.Web.Models;
using System;

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

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.EmployeeType, opt => opt.MapFrom((src, dest) =>
                {
                    var isManager = src.IsManager;
                    var isCEO = src.IsCEO;

                    if (!isManager && !isCEO) return EmployeeType.Employee;
                    if (isManager) return EmployeeType.Manager;
                    return EmployeeType.CEO;
                }));

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