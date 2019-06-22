using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebAPI_BAL;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_DataAccess.WebApiContext;

namespace CodeGenerator
{
    public class Engine
    {
        private readonly string _solutionDirPath;
        public Engine(string solutionDirPath, ServiceProvider serviceProvider)
        {
            _solutionDirPath = solutionDirPath;
            Databases databases = ((IOptions<Databases>)serviceProvider.GetService(typeof(IOptions<Databases>))).Value;

            DatabaseNames = databases.DatabaseNames;
            DatabaseCodeNames = databases.DatabaseCodeNames;
        }

        public List<string> DatabaseNames;
        public List<string> DatabaseCodeNames;

        public string DbContextClassDirectory;
        public string DbContextPublicClassDirectory;
        public string DbContextPrivateClassDirectory;
        public int SelectedDatabaseIndex;

        public (string, string) SelectDatabase(ConsoleKey pressedKey)
        {
            switch (pressedKey)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    SelectedDatabaseIndex = 0;
                    DbContextClassDirectory = Constants.Db1DbContextInterfaceDirectory;
                    DbContextPublicClassDirectory = Constants.Db1PublicClassDirectory;
                    DbContextPrivateClassDirectory = Constants.Db1PrivateClassDirectory;
                    return DatabaseNames.Any() ? (DatabaseNames[0], DatabaseCodeNames[0]) : ("", "");

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    SelectedDatabaseIndex = 1;
                    DbContextClassDirectory = Constants.Db2DbContextInterfaceDirectory;
                    DbContextPublicClassDirectory = Constants.Db2PublicClassDirectory;
                    DbContextPrivateClassDirectory = Constants.Db2PrivateClassDirectory;
                    return DatabaseNames.Count() > 1 ? (DatabaseNames[1], DatabaseCodeNames[1]) : ("", "");

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    SelectedDatabaseIndex = 2;
                    DbContextClassDirectory = Constants.Db3DbContextInterfaceDirectory;
                    DbContextPublicClassDirectory = Constants.Db3PublicClassDirectory;
                    DbContextPrivateClassDirectory = Constants.Db3PrivateClassDirectory;
                    return DatabaseNames.Count() > 2 ? (DatabaseNames[2], DatabaseCodeNames[2]) : ("", "");

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    SelectedDatabaseIndex = 3;
                    DbContextClassDirectory = Constants.Db4DbContextInterfaceDirectory;
                    DbContextPublicClassDirectory = Constants.Db4PublicClassDirectory;
                    DbContextPrivateClassDirectory = Constants.Db4PrivateClassDirectory;
                    return DatabaseNames.Count() > 3 ? (DatabaseNames[3], DatabaseCodeNames[3]) : ("", "");

                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    SelectedDatabaseIndex = 4;
                    DbContextClassDirectory = Constants.Db5DbContextInterfaceDirectory;
                    DbContextPublicClassDirectory = Constants.Db5PublicClassDirectory;
                    DbContextPrivateClassDirectory = Constants.Db5PrivateClassDirectory;
                    return DatabaseNames.Count() > 4 ? (DatabaseNames[4], DatabaseCodeNames[4]) : ("", "");

                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    SelectedDatabaseIndex = 5;
                    DbContextClassDirectory = Constants.Db6DbContextInterfaceDirectory;
                    DbContextPublicClassDirectory = Constants.Db6PublicClassDirectory;
                    DbContextPrivateClassDirectory = Constants.Db6PrivateClassDirectory;
                    return DatabaseNames.Count() > 5 ? (DatabaseNames[5], DatabaseCodeNames[5]) : ("", "");

                case ConsoleKey.D7:
                case ConsoleKey.NumPad7:
                    SelectedDatabaseIndex = 6;
                    DbContextClassDirectory = Constants.Db7DbContextInterfaceDirectory;
                    DbContextPublicClassDirectory = Constants.Db7PublicClassDirectory;
                    DbContextPrivateClassDirectory = Constants.Db7PrivateClassDirectory;
                    return DatabaseNames.Count() > 6 ? (DatabaseNames[6], DatabaseCodeNames[6]) : ("", "");

                case ConsoleKey.D8:
                case ConsoleKey.NumPad8:
                    SelectedDatabaseIndex = 7;
                    DbContextClassDirectory = Constants.Db8DbContextInterfaceDirectory;
                    DbContextPublicClassDirectory = Constants.Db8PublicClassDirectory;
                    DbContextPrivateClassDirectory = Constants.Db8PrivateClassDirectory;
                    return DatabaseNames.Count() > 7 ? (DatabaseNames[7], DatabaseCodeNames[7]) : ("", "");

                case ConsoleKey.D9:
                case ConsoleKey.NumPad9:
                    SelectedDatabaseIndex = 8;
                    DbContextClassDirectory = Constants.Db9DbContextInterfaceDirectory;
                    DbContextPublicClassDirectory = Constants.Db9PublicClassDirectory;
                    DbContextPrivateClassDirectory = Constants.Db9PrivateClassDirectory;
                    return DatabaseNames.Count() > 8 ? (DatabaseNames[8], DatabaseCodeNames[8]) : ("", "");

                default:
                    return ("", "");
            }
        }

        public bool CheckAPiFileExits(string filePath, string versionDir, string controllerName, out string controllerPath)
        {
            controllerPath = "";

            filePath = filePath.Replace("[VERSIONDIR]", versionDir);
            string directoryPath = Path.Combine(GetProjectDirectory(), filePath);
            string[] files = Directory.GetFiles(directoryPath, controllerName + ".cs", SearchOption.AllDirectories);
            if (files.Any())
            {
                controllerPath = filePath = files.FirstOrDefault();
                return File.Exists(filePath);
            }
            return false;
        }

        public bool CheckAPiServiceFileExits(string filePath, string serviceFolderName, string serviceName, out string serviceClassPath, out string serviceInterfacePath)
        {
            serviceClassPath = serviceInterfacePath = "";
            filePath = filePath.Replace("[SERVICEDIRNAME]", serviceFolderName);
            serviceInterfacePath = Path.Combine(GetProjectDirectory(), filePath, "Interfaces",
                "I" + serviceName + "Service.cs");
            serviceClassPath = filePath = Path.Combine(GetProjectDirectory(), filePath, serviceName + "Service.cs");
            return File.Exists(filePath);
        }

        public void WriteToFile(string filePath, string text, string model, string databaseName, string serviceDir = "", string serviceName = "")
        {
            filePath = filePath.Replace("[MODEL]", model).Replace("[DATABASENAME]", databaseName);
            filePath = filePath.Replace("[SERVICEDIR]", serviceDir).Replace("[SERVICENAME]", serviceName);
            filePath = Path.Combine(GetProjectDirectory(), filePath);
            System.IO.Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf('\\')));

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, text);
                Console.WriteLine($"{filePath.Substring(filePath.LastIndexOf('\\') + 1)} created");
            }
            else
            {
                Console.WriteLine($"File \"{filePath}\" already exists.");
            }
        }

        public void UpdateToFile(string filePath, string text, string model, string databaseName, string keyword, string spaces)
        {
            filePath = filePath.Replace("[MODEL]", model).Replace("[DATABASENAME]", databaseName);
            filePath = Path.Combine(GetProjectDirectory(), filePath);
            System.IO.Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf('\\')));

            if (!File.Exists(filePath)) Console.WriteLine($"File \"{filePath}\" does not exists.");

            string fileValue = File.ReadAllText(filePath);
            fileValue = fileValue.Replace(keyword, text + "\n" + spaces + keyword);
            File.WriteAllText(filePath, fileValue);

            Console.WriteLine($"{filePath.Substring(filePath.LastIndexOf('\\') + 1)} Updated");
        }

        public void UpdateApiEndPointFile(string filePath, string text, string keyword)
        {
            filePath = Path.Combine(GetProjectDirectory(), filePath);
            string fileValue = File.ReadAllText(filePath);
            fileValue = fileValue.Replace(keyword, text + "\t\t" + keyword);
            File.WriteAllText(filePath, fileValue);

            Console.WriteLine($"{filePath.Substring(filePath.LastIndexOf('\\') + 1)} Updated");
        }

        public void UpdateApiEndPointFile(string filePath, string text)
        {
            filePath = Path.Combine(GetProjectDirectory(), filePath);
            List<string> fileValue = File.ReadAllLines(filePath).ToList();

            fileValue.RemoveRange(fileValue.Count() - 2, 2);
            fileValue.AddRange(Regex.Split(text, "\r\n|\r|\n"));
            //fileValue = fileValue.Replace(keyword, text + "\n" + "\t\t" + keyword);
            File.WriteAllLines(filePath, fileValue);

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

                case TemplateType.ApiEndPoint:
                    return ReadFile(Constants.ApiEndPointTemplate);
                case TemplateType.ApiEndPointFunction:
                    return ReadFile(Constants.ApiEndPointFunctionTemplate);
                case TemplateType.ApiEndPointServiceClass:
                    return ReadFile(Constants.ApiEndPointServiceClassTemplate);
                case TemplateType.ApiEndPointServiceInterface:
                    return ReadFile(Constants.ApiEndPointServiceInterfaceTemplate);

                case TemplateType.ApiRequestParamClass:
                    return ReadFile(Constants.ApiRequestParamClassTemplate);
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
            template = Regex.Replace(template, Constants.ApiServiceKeywordSDir, serviceDirKeyword);
            template = Regex.Replace(template, Constants.ApiServiceKeywordSName, serviceNameKeyword);
            template = Regex.Replace(template, Constants.ApiServiceKeywordSBInit, balInitKeyword);
            template = Regex.Replace(template, Constants.ApiServiceKeywordSBCtor, balCtorKeyword);
            template = Regex.Replace(template, Constants.ApiServiceKeywordSBLink, balLinkKeyword);

            return template;
        }

        public string ProcessApiEndPointTemplate(string template, string apiDescription = "", string apiEndPointName = "", string apiHttpMethod = "",
            string apiEndPointFunctionName = "", string apiServiceInterface = "", string apiServiceVariable = "", string apiServiceFunction = "", string apiEndPointNameVariable = "",
            string apiServiceFunctionParameters = "", string apiFunctionParameters = "", string parametersSummary = "", string apiFunctionServiceParameters = "",
            string returnType = "")
        {
            template = Regex.Replace(template, Constants.ApiDescription, apiDescription);
            template = Regex.Replace(template, Constants.ApiEndPointName, apiEndPointName);
            template = Regex.Replace(template, Constants.ApiHttpMethod, apiHttpMethod);
            template = Regex.Replace(template, Constants.ApiEndPointFunctionName, apiEndPointFunctionName);
            template = Regex.Replace(template, Constants.ApiServiceInterface, apiServiceInterface);
            template = Regex.Replace(template, Constants.ApiServiceVariable, apiServiceVariable);
            template = Regex.Replace(template, Constants.ApiServiceFunction, apiServiceFunction);
            template = Regex.Replace(template, Constants.ApiEndPointNameVariable, apiEndPointNameVariable);

            template = Regex.Replace(template, Constants.ApiServiceFunctionParameters, apiServiceFunctionParameters);
            template = Regex.Replace(template, Constants.ApiFunctionParameters, apiFunctionParameters);
            template = Regex.Replace(template, Constants.ParametersSummary, parametersSummary);
            template = Regex.Replace(template, Constants.ApiFunctionServiceParameters, apiFunctionServiceParameters);

            template = Regex.Replace(template, Constants.ReturnType, returnType);

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

        public string GetGeneratedModelClass(string databaseName, string tableName, ServiceProvider serviceProvider, string tableModelName)
        {
            dynamic spBal = GetCommonBusinessLogic(SelectedDatabaseIndex, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetModelClassParam, GeneratedModelClassViewModel,
                    GetModelClassParamViewModel>(new GetModelClassParamViewModel() { TableName = tableName, ModelName = tableModelName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedViewModelClass(string databaseName, string tableName, ServiceProvider serviceProvider, string tableModelName)
        {
            dynamic spBal = GetCommonBusinessLogic(SelectedDatabaseIndex, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetViewModelClassParam, GeneratedModelClassViewModel,
                    GetModelClassParamViewModel>(new GetModelClassParamViewModel() { TableName = tableName, ModelName = tableModelName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedSpParamModelClass(string databaseName, string spName, string modelName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(SelectedDatabaseIndex, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetSpParamModelClassParam, GeneratedModelClassViewModel,
                    GetSpModelClassParamViewModel>(new GetSpModelClassParamViewModel()
                    { SpName = spName, ModelName = modelName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedSpParamViewModelClass(string databaseName, string spName, string modelName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(SelectedDatabaseIndex, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetSpParamViewModelClassParam, GeneratedModelClassViewModel,
                    GetSpModelClassParamViewModel>(new GetSpModelClassParamViewModel()
                    { SpName = spName, ModelName = modelName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedSpReturnModelClass(string databaseName, string spName, string modelName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(SelectedDatabaseIndex, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetSpReturnModelClassParam, GeneratedModelClassViewModel,
                    GetSpModelClassParamViewModel>(new GetSpModelClassParamViewModel()
                    { SpName = spName, ModelName = modelName })).ToList();

            return data[0].ModelClass;
        }

        public string GetGeneratedSpReturnViewModelClass(string databaseName, string spName, string modelName, ServiceProvider serviceProvider)
        {
            dynamic spBal = GetCommonBusinessLogic(SelectedDatabaseIndex, serviceProvider);
            List<GeneratedModelClassViewModel> data = ((IEnumerable<GeneratedModelClassViewModel>)spBal
                .ExecuteStoreProcedure<GeneratedModelClass, GetSpReturnViewModelClassParam, GeneratedModelClassViewModel,
                    GetSpModelClassParamViewModel>(new GetSpModelClassParamViewModel()
                    { SpName = spName, ModelName = modelName })).ToList();

            return data[0].ModelClass;
        }

        private dynamic GetCommonBusinessLogic(int selectedIndexDatabase, ServiceProvider serviceProvider)
        {
            switch (selectedIndexDatabase)
            {
                case 0:
                    return serviceProvider.GetService<ICommonStoreProcBusinessLogic<IWebApiDbContext>>();

                case 1:
                    return serviceProvider.GetService<ICommonStoreProcBusinessLogic<INorthwindDbContext>>();

                case 2:
                    return null;

                case 3:
                    return null;

                case 4:
                    return null;

                case 5:
                    return null;

                case 6:
                    return null;

                case 7:
                    return null;

                case 8:
                    return null;

                default:
                    return null;

            }
        }
    }
}
