using AutoMapper;
using Dapper.Identity.Stores;
using WebAPI_Model;
using WebAPI_Model.Test;
using WebAPI_ViewModel.DTO;
using WebAPI_ViewModel.Identity;

namespace WebAPI_Server.AppStart
{
    internal class MappingProfile : Profile
    {
        internal MappingProfile()
        {
            #region Identity
            CreateMap<RegisterUserViewModel, ApplicationUser>().ForMember(x => x.UserName, y => y.MapFrom(map => map.Email ?? string.Empty)).ReverseMap();
            CreateMap<AddNewUserViewModel, ApplicationUser>().ForMember(x => x.UserName, y => y.MapFrom(map => map.Email ?? string.Empty)).ReverseMap();
            CreateMap<LoginUserViewModel, ApplicationUser>().ForMember(x => x.UserName, y => y.MapFrom(map => map.Email ?? string.Empty)).ReverseMap();
            CreateMap<ApplicationRoleViewModel, ApplicationRole>().ReverseMap();
            CreateMap<TestRepo, TestRepoViewModel>().ReverseMap();
            #endregion

            #region Auto Generated Code. Don't Delete or Modify this section
            #region WebApiDb Mapper AG
            //[AUTO_GENERATED_MAPPER_WebApiDb]
            #endregion
            #region Northwind Mapper AG
            CreateMap<CategoriesModel, CategoriesViewModel>().ReverseMap();
			CreateMap<EmployeesModel, EmployeesViewModel>().ReverseMap();
			CreateMap<ShippersModel, ShippersViewModel>().ReverseMap();
			CreateMap<CustomersModel, CustomersViewModel>().ReverseMap();
			CreateMap<OrdersModel, OrdersViewModel>().ReverseMap();
			CreateMap<OrderDetailsModel, OrderDetailsViewModel>().ReverseMap();
			CreateMap<ProductsModel, ProductsViewModel>().ReverseMap();
			CreateMap<SuppliersModel, SuppliersViewModel>().ReverseMap();
            //[AUTO_GENERATED_MAPPER_Northwind]
            #endregion
            #region WebApi Store Procedures
            #region Parameters Mapper
            //[AUTO_GENERATED_SPPARAM_MAPPER_WebApiDb]
            #endregion
            #region Return Mappers
            //[AUTO_GENERATED_SPRETURN_MAPPER_WebApiDb]
            #endregion
            #endregion
            #region Northwind Store Procedures
            #region Parameters Mapper
            //[AUTO_GENERATED_SPPARAM_MAPPER_Northwind]
            #endregion
            #region Return Mappers
            //[AUTO_GENERATED_SPRETURN_MAPPER_Northwind]
            #endregion
            #endregion
            #endregion
        }
    }
}
