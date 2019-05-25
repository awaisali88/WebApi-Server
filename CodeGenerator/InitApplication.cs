using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using WebAPI_BAL;
using WebAPI_DataAccess;
using WebAPI_DataAccess.WebApiContext;
using WebAPI_DataAccess.NorthwindContext;

namespace CodeGenerator
{
    public static class InitApplication
    {
        public static ServiceProvider InitApp()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mappingConfig.CreateMapper();
            string webApiDbConnString = configuration.GetConnectionString("WebApiConnection");
            string northWindConnString = configuration.GetConnectionString("NorthWindConnection");

            var databaseSection = configuration.GetSection(nameof(Databases));

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                //.AddTransient(typeof(ICommonBusinessLogic<,,>), typeof(CommonBusinessLogic<,,>))
                .AddTransient(typeof(ICommonStoreProcBusinessLogic<>), typeof(CommonStoreProcBusinessLogic<>))
                .AddScoped<IWebApiDbContext, WebApiDbContext>()
                .AddScoped<INorthwindDbContext, NorthwindDbContext>()
                .AddSingleton(mapper)
                .AddHttpContextAccessor()
                .AddScoped<IHostingEnvironment, MockHostingEnvironment>()
                .Configure<WebApiDbOptions>(options =>
                {
                    options.LogQuery = true;
                    options.ConnectionString = webApiDbConnString;
                    options.SqlProvider = SqlProvider.MSSQL;
                    options.UseQuotationMarks = true;
                })
                .Configure<NorthwindDbOptions>(options =>
                {
                    options.LogQuery = true;
                    options.ConnectionString = northWindConnString;
                    options.SqlProvider = SqlProvider.MSSQL;
                    options.UseQuotationMarks = true;
                })
                .Configure<Databases>(databaseSection)
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            logger.LogDebug("All done!");

            return serviceProvider;
        }
    }

    internal class MappingProfile : Profile
    {
        internal MappingProfile()
        {
            CreateMap<GetModelClassParam, GetModelClassParamViewModel>().ReverseMap();
            CreateMap<GetViewModelClassParam, GetModelClassParamViewModel>().ReverseMap();
            CreateMap<GeneratedModelClass, GeneratedModelClassViewModel>().ReverseMap();

            CreateMap<GetSpParamModelClassParam, GetSpModelClassParamViewModel>().ReverseMap();
            CreateMap<GetSpParamViewModelClassParam, GetSpModelClassParamViewModel>().ReverseMap();
            CreateMap<GetSpReturnModelClassParam, GetSpModelClassParamViewModel>().ReverseMap();
            CreateMap<GetSpReturnViewModelClassParam, GetSpModelClassParamViewModel>().ReverseMap();
        }
    }

    public class MockHostingEnvironment : IHostingEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string WebRootPath { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }

    public class Databases
    {
        public string DatabaseName { get; set; }
        public string DatabaseCodeName { get; set; }

        public List<string> DatabaseNames => !string.IsNullOrEmpty(DatabaseName) ? DatabaseName.Split(",").ToList() : null;
        public List<string> DatabaseCodeNames => !string.IsNullOrEmpty(DatabaseCodeName) ? DatabaseCodeName.Split(",").ToList() : null;
    }
}