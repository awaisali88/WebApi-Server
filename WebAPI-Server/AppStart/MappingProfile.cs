using AutoMapper;
using Dapper.Identity.Stores;
using WebAPI_Model.Test;
using WebAPI_ViewModel.DTO;
using WebAPI_ViewModel.Identity;

namespace WebAPI_Server.AppStart
{
    internal class MappingProfile : Profile
    {
        internal MappingProfile()
        {
            CreateMap<RegisterUserViewModel, ApplicationUser>().ForMember(x => x.UserName, y => y.MapFrom(map => map.Email ?? string.Empty)).ReverseMap();
            CreateMap<AddNewUserViewModel, ApplicationUser>().ForMember(x => x.UserName, y => y.MapFrom(map => map.Email ?? string.Empty)).ReverseMap();
            CreateMap<LoginUserViewModel, ApplicationUser>().ForMember(x => x.UserName, y => y.MapFrom(map => map.Email ?? string.Empty)).ReverseMap();
            CreateMap<ApplicationRoleViewModel, ApplicationRole>().ReverseMap();
            CreateMap<TestRepo, TestRepoViewModel>().ReverseMap();
        }
    }
}
