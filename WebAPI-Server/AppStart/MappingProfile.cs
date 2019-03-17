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
            CreateMap<RegisterUserViewModel, ApplicationUser>().ForMember(x => x.UserName, y => y.MapFrom(map => map.Email ?? string.Empty)).ReverseMap();
            CreateMap<AddNewUserViewModel, ApplicationUser>().ForMember(x => x.UserName, y => y.MapFrom(map => map.Email ?? string.Empty)).ReverseMap();
            CreateMap<LoginUserViewModel, ApplicationUser>().ForMember(x => x.UserName, y => y.MapFrom(map => map.Email ?? string.Empty)).ReverseMap();
            CreateMap<ApplicationRoleViewModel, ApplicationRole>().ReverseMap();
            CreateMap<TestRepo, TestRepoViewModel>().ReverseMap();

            CreateMap<TestTicketCustomProcedureParam, TestTicketCustomProcedureParamViewModel>().ReverseMap();
            CreateMap<PROC_Ticket_Custom_Search_Model, PROC_Ticket_Custom_Search_ViewModel>().ReverseMap();


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
            #endregion
        }
    }
}
