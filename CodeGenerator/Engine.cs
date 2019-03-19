using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using Dapper.Repositories.DbContext;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using WebAPI_BAL;
using WebAPI_DataAccess.WebApiContext;
using WebAPI_DataAccess.NorthwindContext;

namespace CodeGenerator
{
    public class Engine
    {
        private readonly string _solutionDirPath;
        public Engine(string solutionDirPath)
        {
            _solutionDirPath = solutionDirPath;
        }

        public readonly string WebApiDbDatabaseName = "WebApiDb";
        public readonly string NorthwindDatabaseName = "Northwind";
        public readonly string WebApiDbCodeName = "WebApi";
        public readonly string NorthwindCodeName = "Northwind";

        public bool WriteToFile(string filePath, string text, string model, string databaseName, string serviceDir = "", string serviceName = "")
        {
            filePath = filePath.Replace("[MODEL]", model).Replace("[DATABASENAME]", databaseName);
            filePath = filePath.Replace("[SERVICEDIR]", serviceDir).Replace("[SERVICENAME]", serviceName);
            filePath = Path.Combine(GetProjectDirectory(), filePath);
            System.IO.Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf('\\')));

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, text);
                Console.WriteLine($"{filePath.Substring(filePath.LastIndexOf('\\') + 1)} created");
                return true;
            }
            else
            {
                Console.WriteLine($"File \"{filePath}\" already exists.");
                return false;
            }
        }

        public void UpdateToFile(string filePath, string text, string model, string databaseName, string keyword, string spaces)
        {
            filePath = filePath.Replace("[MODEL]", model).Replace("[DATABASENAME]", databaseName);
            filePath = Path.Combine(GetProjectDirectory(), filePath);
            System.IO.Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf('\\')));

            if (!File.Exists(filePath)) Console.WriteLine($"File \"{filePath}\" already exists.");

            string fileValue = File.ReadAllText(filePath);
            fileValue = fileValue.Replace(keyword, text + "\n" + spaces + keyword);
            File.WriteAllText(filePath, fileValue);

            Console.WriteLine($"{filePath.Substring(filePath.LastIndexOf('\\') + 1)} Updated");
        }

        public string ReadTemplate(TemplateType type)
        {
            switch (type)
            {
                case TemplateType.Bal:
                    return ReadFile(Constants.BalTemplate);
                case TemplateType.Mapper:
                    return ReadFile(Constants.MapperTemplate);
                case TemplateType.IBal:
                    return ReadFile(Constants.IBalTemplate);
                case TemplateType.Model:
                    return ReadFile(Constants.ModelTemplate);
                case TemplateType.ViewModel:
                    return ReadFile(Constants.ViewModelTemplate);
                case TemplateType.Repository:
                    return ReadFile(Constants.RepositoryTemplate);
                case TemplateType.Validator:
                    return ReadFile(Constants.ValidatorTemplate);
                case TemplateType.ApiService:
                    return ReadFile(Constants.ApiServiceTemplate);
                case TemplateType.IApiService:
                    return ReadFile(Constants.IApiServiceTemplate);

                case TemplateType.SpParamModel:
                    return ReadFile(Constants.SpParamModelTemplate);
                case TemplateType.SpParamViewModel:
                    return ReadFile(Constants.SpParamViewModelTemplate);
                case TemplateType.SpReturnModel:
                    return ReadFile(Constants.SpReturnModelTemplate);
                case TemplateType.SpReturnViewModel:
                    return ReadFile(Constants.SpReturnViewModelTemplate);

                default:
                    return "";
            }
        }

        public string ProcessTemplate(string template, string modelKeyword = "", string databaseKeyword = "",
            string modelClassKeyword = "", string viewModelClassKeyword = "")
        {
            template = Regex.Replace(template, Constants.ModelTemKeyword, modelKeyword);
            template = Regex.Replace(template, Constants.ModelClassTemKeyword, modelClassKeyword);
            template = Regex.Replace(template, Constants.DatabaseTemKeyword, databaseKeyword);
            template = Regex.Replace(template, Constants.ModelCcTemKeyword, modelKeyword.ToCamelCase());
            template = Regex.Replace(template, Constants.ViewModelClassTemKeyword, viewModelClassKeyword);

            return template;
        }

        public string ProcessApiServiceTemplate(string template, string serviceDirKeyword = "", string serviceNameKeyword = "",
            string balInitKeyword = "", string balCtorKeyword = "", string balLinkKeyword = "")
        {
            template = Regex.Replace(template, Constants.ApiServiceKeywordSDIR, serviceDirKeyword);
            template = Regex.Replace(template, Constants.ApiServiceKeywordSNAME, serviceNameKeyword);
            template = Regex.Replace(template, Constants.ApiServiceKeywordSBINIT, balInitKeyword);
            template = Regex.Replace(template, Constants.ApiServiceKeywordSBCTOR, balCtorKeyword);
            template = Regex.Replace(template, Constants.ApiServiceKeywordSBLINK, balLinkKeyword);

            return template;
        }

        public string ReadFile(string filePath)
        {
            filePath = Path.Combine(GetProjectDirectory(), filePath);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            return "";
        }

        private string GetProjectDirectory()
        {
            //string dir = Environment.CurrentDirectory.Substring(0,
            //    Environment.CurrentDirectory.LastIndexOf(GetType().Assembly.GetName().Name,
            //        StringComparison.InvariantCulture) + GetType().Assembly.GetName().Name.Length);
            return _solutionDirPath;
        }

        public string GetGeneratedModelClass(string databaseName, string tableName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(databaseName, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetModelClassParam, GeneratedModelClassViewModel,
                    GetModelClassParamViewModel>(new GetModelClassParamViewModel() { TableName = tableName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedViewModelClass(string databaseName, string tableName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(databaseName, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetViewModelClassParam, GeneratedModelClassViewModel,
                    GetModelClassParamViewModel>(new GetModelClassParamViewModel() { TableName = tableName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedSpParamModelClass(string databaseName, string spName, string modelName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(databaseName, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetSpParamModelClassParam, GeneratedModelClassViewModel,
                    GetSpModelClassParamViewModel>(new GetSpModelClassParamViewModel()
                    { SpName = spName, ModelName = modelName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedSpParamViewModelClass(string databaseName, string spName, string modelName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(databaseName, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetSpParamViewModelClassParam, GeneratedModelClassViewModel,
                    GetSpModelClassParamViewModel>(new GetSpModelClassParamViewModel()
                    { SpName = spName, ModelName = modelName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedSpReturnModelClass(string databaseName, string spName, string modelName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(databaseName, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetSpReturnModelClassParam, GeneratedModelClassViewModel,
                    GetSpModelClassParamViewModel>(new GetSpModelClassParamViewModel()
                    { SpName = spName, ModelName = modelName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedSpReturnViewModelClass(string databaseName, string spName, string modelName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(databaseName, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetSpReturnViewModelClassParam, GeneratedModelClassViewModel,
                    GetSpModelClassParamViewModel>(new GetSpModelClassParamViewModel()
                    { SpName = spName, ModelName = modelName })).ToList();

            return data[0].ModelClass;
        }


        private dynamic GetCommonBusinessLogic(string databaseName, ServiceProvider serviceProvider)
        {
            if (databaseName == WebApiDbDatabaseName)
                return serviceProvider.GetService<ICommonStoreProcBusinessLogic<IWebApiDbContext>>();

            if (databaseName == NorthwindDatabaseName)
                return serviceProvider.GetService<ICommonStoreProcBusinessLogic<INorthwindDbContext>>();

            return null;
        }
    }
}
