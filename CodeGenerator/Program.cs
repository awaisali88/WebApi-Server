using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("...Welcome to Code Generator...");
            Console.WriteLine("======================================");
            try
            {
                var serviceProvider = InitApplication.InitApp();
                bool isContinue = true;

                #region Directory Select
                //string dirPath = @"D:\My_Data\Projects\LiveAdmins\WGApi\wgapi\CodeGenerator\";
                CodeGeneratorForDirectory(out var dirPath, out isContinue);
                #endregion

                while (isContinue)
                {
                    Console.Clear();
                    Engine engine = new Engine(dirPath, serviceProvider);

                    #region Database select
                    var databaseResult = CodeGeneratorForDatabaseSelect(out var selectedDatabase, out var databaseName,
                        out var databaseCodeName, ref engine);
                    if (!databaseResult) break;
                    #endregion

                    #region Mode select
                    CodeGeneratorModeSelect(out var selectedMode);
                    #endregion

                    //Generate code for Database Table
                    CodeGeneratorForDatabaseTables(selectedMode, serviceProvider, engine,
                        databaseName, databaseCodeName);

                    //Generate code for store procedure
                    CodeGeneratorForStoreProcedure(selectedMode, serviceProvider, engine,
                        databaseName, databaseCodeName);

                    //Generate code for API Service
                    CodeGeneratorForApiService(selectedMode, engine, databaseName);

                    //Generate code for Api End Point
                    CodeGeneratorForApiEndPoint(selectedMode, engine);

                    //Generate API Request Param Class
                    CodeGeneratorForApiRequestParamClass(selectedMode, engine, databaseCodeName);

                    Console.WriteLine();
                    Console.Write("Do you want to add another model? Y/N: ");
                    isContinue = Console.ReadKey(true).Key == ConsoleKey.Y;
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.Write("Press <Enter> to exit... ");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                {
                }
                Console.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
                Console.Write("Press <Enter> to exit... ");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                {
                }
                Console.Clear();
            }
        }

        private static void CodeGeneratorForDirectory(out string dirPath, out bool isContinue)
        {
            bool isContinueDirPath = true;
            isContinue = true;

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0,
                System.Reflection.Assembly.GetExecutingAssembly().Location.IndexOf("CodeGenerator", StringComparison.Ordinal));
            if (!string.IsNullOrEmpty(path))
            {
                dirPath = path;
                return;
            }

            do
            {
                Console.WriteLine("Please provide full directory path of CodeGenerator project solution");
                Console.WriteLine(@"eg: D:\My_Data\Projects\LiveAdmins\WGApi\wgapi\");
                Console.WriteLine();
                Console.WriteLine("Enter Directory Path:");
                if (!CancelableReadLine(out dirPath))
                {
                    isContinue = false;
                    return;
                }

                //dirPath = Console.ReadLine();
                if (!string.IsNullOrEmpty(dirPath))
                {
                    Console.Clear();
                    isContinueDirPath = false;
                }
                else
                    Console.Clear();
            } while (isContinueDirPath);
        }

        private static bool CodeGeneratorForDatabaseSelect(out ConsoleKey selectedDatabase, out string databaseName, out string databaseCodeName, ref Engine engine)
        {
            bool isContinueForDatabase = true;
            do
            {
                string databaseSelectionString = "Available databases:";
                int selectNumber = 1;
                engine.DatabaseNames.ForEach(val => { databaseSelectionString += $"\n{selectNumber++}. {val}"; });

                Console.WriteLine(databaseSelectionString);
                Console.WriteLine();
                Console.Write("Please select database:\t");

                selectedDatabase = Console.ReadKey(true).Key;
                if (!string.IsNullOrEmpty(engine.SelectDatabase(selectedDatabase).Item1))
                {
                    isContinueForDatabase = false;
                    Console.WriteLine(SelectDatabase(selectedDatabase, engine).Item1);
                    Console.WriteLine("======================================");
                }
                else
                    Console.Clear();
            } while (isContinueForDatabase);

            (databaseName, databaseCodeName) = SelectDatabase(selectedDatabase, engine);

            return true;
        }

        private static (string, string) SelectDatabase(ConsoleKey pressedKey, Engine e)
        {
            return e.SelectDatabase(pressedKey);
        }

        private static void CodeGeneratorModeSelect(out ConsoleKey selectedMode)
        {
            bool isContinueForMode = true;
            do
            {
                Console.WriteLine("Available Generators:\n1. Table\n2. Store Procedure\n3. Api Service\n4. Api End Point\n5. Api Request Param Class");
                Console.WriteLine();
                Console.Write("Please select mode:\t");

                selectedMode = Console.ReadKey(true).Key;
                if (selectedMode == ConsoleKey.D1 || selectedMode == ConsoleKey.NumPad1 ||
                    selectedMode == ConsoleKey.D2 || selectedMode == ConsoleKey.NumPad2 ||
                    selectedMode == ConsoleKey.D3 || selectedMode == ConsoleKey.NumPad3 ||
                    selectedMode == ConsoleKey.D4 || selectedMode == ConsoleKey.NumPad4 ||
                    selectedMode == ConsoleKey.D5 || selectedMode == ConsoleKey.NumPad5)
                {
                    isContinueForMode = false;
                    Console.WriteLine(SelectMode(selectedMode));
                    Console.WriteLine("======================================");
                }
                else
                    Console.Clear();
            } while (isContinueForMode);
        }

        private static string SelectMode(ConsoleKey pressedKey)
        {
            return pressedKey == ConsoleKey.D1 || pressedKey == ConsoleKey.NumPad1
                ? "Table"
                : pressedKey == ConsoleKey.D2 || pressedKey == ConsoleKey.NumPad2
                    ? "StoreProcedure"
                    : pressedKey == ConsoleKey.D3 || pressedKey == ConsoleKey.NumPad3
                        ? "Api Service"
                        : pressedKey == ConsoleKey.D4 || pressedKey == ConsoleKey.NumPad4
                            ? "Api End Point"
                            : pressedKey == ConsoleKey.D5 || pressedKey == ConsoleKey.NumPad5
                                ? "Api Request Param Class"
                    : "";
        }

        private static void CodeGeneratorForDatabaseTables(ConsoleKey selectedMode, ServiceProvider serviceProvider,
            Engine engine, string databaseName, string databaseCodeName)
        {
            if (selectedMode == ConsoleKey.D1 || selectedMode == ConsoleKey.NumPad1)
            {
                bool isContinueForTable = true;
                string fullTableName = "";
                string tableModelName = "";
                do
                {
                    Console.WriteLine(
                        "Enter fully qualified table name eg: Schema.TableName (dbo.Users)");
                    if (!CancelableReadLine(out fullTableName)) return;
                    //fullTableName = Console.ReadLine();
                    Console.WriteLine();

                    Console.WriteLine(
                        "Enter model name for the table (press enter to skip) eg: Users");
                    if (!CancelableReadLine(out tableModelName)) return;
                    //fullTableName = Console.ReadLine();

                    Console.WriteLine("======================================");
                    if (fullTableName != null && fullTableName.Split('.').Length == 2)
                    {
                        isContinueForTable = false;
                    }
                } while (isContinueForTable);

                string schemaName = fullTableName.Split('.')[0];
                string model, apiServiceName;
                var tableName = fullTableName.Split('.')[1];
                if (string.IsNullOrEmpty(tableModelName)) tableModelName = tableName;
                model = apiServiceName = tableModelName;

                Console.WriteLine("Generating Model Class Code");
                string generatedModelClass = engine.GetGeneratedModelClass(databaseName,
                    $"{schemaName}.{tableName}", serviceProvider, model);
                Console.WriteLine("Generating ViewModel Class Code");
                string generatedViewModelClass = engine.GetGeneratedViewModelClass(databaseName,
                    $"{schemaName}.{tableName}", serviceProvider, model);

                string apiServiceFolderName = databaseName;
                string balInitTemp = "\t\tprivate readonly [SB] _[SBC];\n";
                string balInit = "";
                string balCtorTemp = ", [SB] [SBC]";
                string balCtor = "";
                string balLinkTemp = "\n\t\t\t_[SBC] = [SBC];";
                string balLink = "";

                string[] selectedBal = $"I{model}Bal".Split(",");
                foreach (var sBal in selectedBal)
                {
                    var sbCamelCase = sBal.Substring(1).ToCamelCase();
                    balInit += balInitTemp.Replace("[SB]", sBal).Replace("[SBC]", sbCamelCase);
                    balCtor += balCtorTemp.Replace("[SB]", sBal).Replace("[SBC]", sbCamelCase);
                    balLink += balLinkTemp.Replace("[SBC]", sbCamelCase).Replace("[SBC]", sbCamelCase);
                }

                Console.WriteLine("Generating files");
                //Create Model Class
                string modelTem = engine.ReadTemplate(TemplateType.Model);
                modelTem = engine.ProcessTemplate(modelTem, model, databaseCodeName, generatedModelClass,
                    generatedViewModelClass);
                engine.WriteToFile(Constants.ModelClassDirectory, modelTem, model, databaseName);

                //Create ViewModel Class
                string viewModelTem = engine.ReadTemplate(TemplateType.ViewModel);
                viewModelTem = engine.ProcessTemplate(viewModelTem, model, databaseCodeName,
                    generatedModelClass,
                    generatedViewModelClass);
                engine.WriteToFile(Constants.ViewModelClassDirectory, viewModelTem, model, databaseName);

                //Create Validator Class
                string validatorTem = engine.ReadTemplate(TemplateType.Validator);
                validatorTem = engine.ProcessTemplate(validatorTem, model, databaseCodeName,
                    generatedModelClass,
                    generatedViewModelClass);
                engine.WriteToFile(Constants.ValidatorClassDirectory, validatorTem, model, databaseName);

                //Create IBal Interface
                string balInterfaceTem = engine.ReadTemplate(TemplateType.IBal);
                balInterfaceTem = engine.ProcessTemplate(balInterfaceTem, model, databaseCodeName,
                    generatedModelClass,
                    generatedViewModelClass);
                engine.WriteToFile(Constants.BalInterfaceDirectory, balInterfaceTem, model, databaseName);

                //Create Bal Class
                string balClassTem = engine.ReadTemplate(TemplateType.Bal);
                balClassTem = engine.ProcessTemplate(balClassTem, model, databaseCodeName, generatedModelClass,
                    generatedViewModelClass);
                engine.WriteToFile(Constants.BalClassDirectory, balClassTem, model, databaseName);

                //Create Api Service Interface
                string serviceInterfaceTem = engine.ReadTemplate(TemplateType.IApiService);
                serviceInterfaceTem = engine.ProcessApiServiceTemplate(serviceInterfaceTem, apiServiceFolderName, apiServiceName,
                    balInit, balCtor, balLink);
                engine.WriteToFile(Constants.ApiServiceInterfaceDirectory, serviceInterfaceTem, "", databaseName, apiServiceFolderName, apiServiceName);

                //Create Api Service Class
                string serviceTem = engine.ReadTemplate(TemplateType.ApiService);
                serviceTem = engine.ProcessApiServiceTemplate(serviceTem, apiServiceFolderName, apiServiceName,
                    balInit, balCtor, balLink);
                engine.WriteToFile(Constants.ApiServiceDirectory, serviceTem, "", databaseName, apiServiceFolderName, apiServiceName);

                //Update Context Files
                string repoClassTemp = engine.ReadTemplate(TemplateType.Repository);
                repoClassTemp = engine.ProcessTemplate(repoClassTemp, model, databaseCodeName,
                    generatedModelClass,
                    generatedViewModelClass);

                string repoInterface = repoClassTemp.Split('\n')[0]
                    .Substring(0, repoClassTemp.Split('\n')[0].Length - 1);
                string repoPrivate = repoClassTemp.Split('\n')[1]
                    .Substring(0, repoClassTemp.Split('\n')[1].Length - 1);
                string repoPublic = repoClassTemp.Split('\n')[2]
                    .Substring(0, repoClassTemp.Split('\n')[2].Length - 1);

                string repoKeyword = Constants.RepositoryKeyword.Replace("[DATABASENAME]", databaseName);
                string initialSpaces = "        ";

                engine.UpdateToFile(engine.DbContextClassDirectory, repoInterface, model,
                    databaseName, repoKeyword, initialSpaces);
                engine.UpdateToFile(engine.DbContextPublicClassDirectory, repoPublic, model,
                    databaseName, repoKeyword, initialSpaces);
                engine.UpdateToFile(engine.DbContextPrivateClassDirectory, repoPrivate, model,
                    databaseName, repoKeyword, initialSpaces);

                //Update Mapper files For BAL
                string mapperClassTemp = engine.ReadTemplate(TemplateType.Mapper);
                mapperClassTemp = engine.ProcessTemplate(mapperClassTemp, model, databaseCodeName,
                    generatedModelClass,
                    generatedViewModelClass);

                string mapperCode = mapperClassTemp.Split('\n')[0]
                    .Substring(0, mapperClassTemp.Split('\n')[0].Length - 1);
                string diCode = mapperClassTemp.Split('\n')[1]
                    .Substring(0, mapperClassTemp.Split('\n')[1].Length - 1);

                string mapperKeyword = Constants.MappingProfileKeyword.Replace("[DATABASENAME]", databaseName);
                string diKeyword = Constants.RegisterServiceKeyword.Replace("[DATABASENAME]", databaseName);
                string mapperSpaces = "\t\t\t";
                engine.UpdateToFile(Constants.MappingProfileClassDirectory, mapperCode, model, databaseName,
                    mapperKeyword, mapperSpaces);
                engine.UpdateToFile(Constants.RegisterServiceClassDirectory, diCode, model, databaseName,
                    diKeyword, mapperSpaces);

                //Update Mapper files of Service
                string mapperClassTempService = engine.ReadTemplate(TemplateType.Mapper);
                mapperClassTempService = engine.ProcessApiServiceTemplate(mapperClassTempService, apiServiceFolderName, apiServiceName,
                    balInit, balCtor, balLink);

                string serviceCode = mapperClassTempService.Split('\n')[2]
                    .Substring(0, mapperClassTempService.Split('\n')[2].Length - 1);

                string serviceKeyword = Constants.RegisterApiServiceKeyword;
                string mapperSpacesService = "\t\t\t";
                engine.UpdateToFile(Constants.RegisterServiceClassDirectory, serviceCode, "", databaseName,
                    serviceKeyword, mapperSpacesService);
            }
        }

        private static void CodeGeneratorForStoreProcedure(ConsoleKey selectedMode, ServiceProvider serviceProvider,
            Engine engine, string databaseName, string databaseCodeName)
        {
            if (selectedMode == ConsoleKey.D2 || selectedMode == ConsoleKey.NumPad2)
            {
                string spName = "";
                string modelName = "";
                bool generateParam = true;
                bool generateModel = true;
                bool isContinueForTable = true;
                do
                {
                    Console.WriteLine(
                        "Enter fully qualified Store Procedure name eg: Schema.procedure (dbo.sp_Users)");

                    if (!CancelableReadLine(out spName)) return;
                    //spName = Console.ReadLine();
                    Console.WriteLine("======================================");
                    if (spName != null && spName.Split('.').Length == 2)
                    {
                        Console.WriteLine(
                            "Enter C# Model name for the procedure eg: Users");

                        if (!CancelableReadLine(out modelName)) return;
                        //modelName = Console.ReadLine();
                        Console.WriteLine("======================================");

                        Console.Write(
                            "Generate return model for the Store procedure Y/N: ");
                        generateModel = Console.ReadKey(true).Key == ConsoleKey.Y;
                        Console.WriteLine("");

                        Console.Write(
                            "Generate parameters model for the Store procedure Y/N: ");
                        generateParam = Console.ReadKey(true).Key == ConsoleKey.Y;
                        Console.WriteLine("\n======================================");

                        isContinueForTable = false;
                    }
                } while (isContinueForTable);

                string generatedSpParamModelClass = "";
                string generatedSpParamViewModelClass = "";
                if (generateParam)
                {
                    Console.WriteLine("Generating Param Model Class Code");
                    generatedSpParamModelClass = engine.GetGeneratedSpParamModelClass(databaseName,
                        spName, modelName, serviceProvider);
                    Console.WriteLine("Generating param ViewModel Class Code");
                    generatedSpParamViewModelClass = engine.GetGeneratedSpParamViewModelClass(databaseName,
                        spName, modelName, serviceProvider);
                }

                string generatedSpReturnModelClass = "";
                if (generateModel)
                {
                    Console.WriteLine("Generating Return Model Class Code");
                    generatedSpReturnModelClass = engine.GetGeneratedSpReturnModelClass(databaseName,
                        spName, modelName, serviceProvider);
                }

                Console.WriteLine("Generating Return ViewModel Class Code");
                string generatedSpReturnViewModelClass = engine.GetGeneratedSpReturnViewModelClass(databaseName,
                    spName, modelName, serviceProvider);

                Console.WriteLine("\nGenerating files");

                if (generateParam)
                {
                    //Create SP Param Model Class
                    string paramModelTem = engine.ReadTemplate(TemplateType.SpParamModel);
                    paramModelTem = engine.ProcessTemplate(paramModelTem, modelName, databaseCodeName, generatedSpParamModelClass,
                        generatedSpParamViewModelClass);
                    engine.WriteToFile(Constants.SpParamModelDirectory, paramModelTem, modelName, databaseName);

                    //Create SP Param ViewModel Class
                    string paramViewModelTem = engine.ReadTemplate(TemplateType.SpParamViewModel);
                    paramViewModelTem = engine.ProcessTemplate(paramViewModelTem, modelName, databaseCodeName, generatedSpParamModelClass,
                        generatedSpParamViewModelClass);
                    engine.WriteToFile(Constants.SpParamViewModelDirectory, paramViewModelTem, modelName, databaseName);

                    //Update StoreProceduresNames Class
                    string spNameTem = $"public const string {modelName}SpName = \"{spName}\";";
                    string spNameKeyword = Constants.SpProcNameKeyword.Replace("[DATABASENAME]", databaseName);
                    string spNameMapperSpaces = "\t\t";
                    engine.UpdateToFile(Constants.SpProcNameDirectory, spNameTem, modelName, databaseName,
                        spNameKeyword, spNameMapperSpaces);
                }

                if (generateModel)
                {
                    //Create SP Return Model Class
                    string returnModelTem = engine.ReadTemplate(TemplateType.SpReturnModel);
                    returnModelTem = engine.ProcessTemplate(returnModelTem, modelName, databaseCodeName, generatedSpReturnModelClass,
                        generatedSpReturnViewModelClass);
                    engine.WriteToFile(Constants.SpReturnModelDirectory, returnModelTem, modelName, databaseName);
                }

                //Create SP Param ViewModel Class
                string returnViewModelTem = engine.ReadTemplate(TemplateType.SpReturnViewModel);
                returnViewModelTem = engine.ProcessTemplate(returnViewModelTem, modelName, databaseCodeName, generatedSpReturnModelClass,
                    generatedSpReturnViewModelClass);
                engine.WriteToFile(Constants.SpReturnViewModelDirectory, returnViewModelTem, modelName, databaseName);

                if (generateModel)
                {
                    //Update Mapper files For BAL
                    string mapperClassTemp = engine.ReadTemplate(TemplateType.Mapper);
                    mapperClassTemp = engine.ProcessTemplate(mapperClassTemp, modelName, databaseCodeName,
                        "",
                        "");

                    string paramMapperCode = mapperClassTemp.Split('\n')[3]
                        .Substring(0, mapperClassTemp.Split('\n')[3].Length - 1);
                    string retrunMapperCode = mapperClassTemp.Split('\n')[4]
                        .Substring(0, mapperClassTemp.Split('\n')[4].Length - 1);

                    string paramMapperKeyword =
                        Constants.SpParamMappingProfileKeyword.Replace("[DATABASENAME]", databaseName);
                    string retrunMapperKeyword =
                        Constants.SpReturnMappingProfileKeyword.Replace("[DATABASENAME]", databaseName);
                    string mapperSpaces = "\t\t\t";
                    if (generateParam)
                        engine.UpdateToFile(Constants.MappingProfileClassDirectory, paramMapperCode, modelName,
                            databaseName,
                            paramMapperKeyword, mapperSpaces);

                    engine.UpdateToFile(Constants.MappingProfileClassDirectory, retrunMapperCode, modelName,
                        databaseName,
                        retrunMapperKeyword, mapperSpaces);
                }

                if (generateParam)
                {
                    string spCode =
                        $"ExecuteStoreProcedure<{modelName}Model, {modelName}Param, {modelName}ViewModel, {modelName}ParamViewModel>(new {modelName}ParamViewModel(){{ }});";
                    Console.WriteLine("======================================");
                    Console.WriteLine($"Use below code [Auto copied to clipboard]:");
                    Console.WriteLine(spCode);
                    TextCopy.Clipboard.SetText(spCode);
                }
            }
        }

        private static void CodeGeneratorForApiService(ConsoleKey selectedMode, Engine engine, string databaseName)
        {
            if (selectedMode == ConsoleKey.D3 || selectedMode == ConsoleKey.NumPad3)
            {
                Console.WriteLine("Enter folder name:\t");
                if (!CancelableReadLine(out string apiServiceFolderName)) return;
                //string apiServiceFolderName = Console.ReadLine();

                Console.WriteLine("Enter Api service name:\t");
                if (!CancelableReadLine(out string apiServiceName)) return;
                //string apiServiceName = Console.ReadLine();

                Console.WriteLine("Enter  one or multiple BAL Interfaces eg. IUserBal,ICustomerBal,...:\t");
                if (!CancelableReadLine(out string balClasses)) return;
                //string balClasses = Console.ReadLine();

                Console.WriteLine("======================================");

                string balInitTemp = "\t\tprivate readonly [SB] _[SBC];\n";
                string balInit = "";
                string balCtorTemp = ", [SB] [SBC]";
                string balCtor = "";
                string balLinkTemp = "\n\t\t\t_[SBC] = [SBC];";
                string balLink = "";

                string[] selectedBal = balClasses.Split(",");
                foreach (var sBal in selectedBal)
                {
                    var sbCamelCase = sBal.Substring(1).ToCamelCase();
                    balInit += balInitTemp.Replace("[SB]", sBal).Replace("[SBC]", sbCamelCase);
                    balCtor += balCtorTemp.Replace("[SB]", sBal).Replace("[SBC]", sbCamelCase);
                    balLink += balLinkTemp.Replace("[SBC]", sbCamelCase).Replace("[SBC]", sbCamelCase);
                }

                Console.WriteLine("Generating files");
                //Create Api Service Interface
                string serviceInterfaceTem = engine.ReadTemplate(TemplateType.IApiService);
                serviceInterfaceTem = engine.ProcessApiServiceTemplate(serviceInterfaceTem, apiServiceFolderName, apiServiceName,
                    balInit, balCtor, balLink);
                engine.WriteToFile(Constants.ApiServiceInterfaceDirectory, serviceInterfaceTem, "", databaseName, apiServiceFolderName, apiServiceName);

                //Create Api Service Class
                string serviceTem = engine.ReadTemplate(TemplateType.ApiService);
                serviceTem = engine.ProcessApiServiceTemplate(serviceTem, apiServiceFolderName, apiServiceName,
                    balInit, balCtor, balLink);
                engine.WriteToFile(Constants.ApiServiceDirectory, serviceTem, "", databaseName, apiServiceFolderName, apiServiceName);

                //Update Mapper files
                string mapperClassTemp = engine.ReadTemplate(TemplateType.Mapper);
                mapperClassTemp = engine.ProcessApiServiceTemplate(mapperClassTemp, apiServiceFolderName, apiServiceName,
                    balInit, balCtor, balLink);

                string serviceCode = mapperClassTemp.Split('\n')[2]
                    .Substring(0, mapperClassTemp.Split('\n')[2].Length - 1);

                string serviceKeyword = Constants.RegisterApiServiceKeyword;
                string mapperSpaces = "\t\t\t";
                engine.UpdateToFile(Constants.RegisterServiceClassDirectory, serviceCode, "", databaseName,
                    serviceKeyword, mapperSpaces);
            }
        }

        private static void CodeGeneratorForApiEndPoint(ConsoleKey selectedMode, Engine engine)
        {
            if (selectedMode == ConsoleKey.D4 || selectedMode == ConsoleKey.NumPad4)
            {
                string controllerVersion = "";
                string controllerName = "";
                string apiDescription = "";
                string apiRoute = "";
                string apiFunctionName = "";
                string apiMethodType = "";
                string apiServiceFolderName = "";
                string apiServiceName = "";
                string apiServiceFunctionName = "";

                string apiParameters = "";
                string returnType = "";

                #region User Input
                Console.WriteLine(
                    "Enter Controller version name eg: v1 or v2 or v3 ...");
                if (!CancelableReadLine(out controllerVersion)) return;
                //controllerVersion = Console.ReadLine();

                Console.WriteLine(
                    "Enter Controller name eg: ChatWindowController");
                if (!CancelableReadLine(out controllerName)) return;
                //controllerName = Console.ReadLine();

                Console.WriteLine("======================================");

                Console.WriteLine(
                    "Enter Api Endpoint description:");
                if (!CancelableReadLine(out apiDescription)) return;
                //apiDescription = Console.ReadLine();

                Console.WriteLine(
                    "Enter Api Endpoint route:");
                if (!CancelableReadLine(out apiRoute)) return;
                //apiRoute = Console.ReadLine();

                Console.WriteLine(
                    "Enter Api function name:");
                if (!CancelableReadLine(out apiFunctionName)) return;
                //apiFunctionName = Console.ReadLine();

                Console.WriteLine(
                    "Enter Api method type: eg Get, Post, Put, Delete, Patch");
                if (!CancelableReadLine(out apiMethodType)) return;
                //apiMethodType = Console.ReadLine();

                Console.WriteLine(
                    "Enter Api parameters (press enter to skip): eg long userId, object abc");
                if (!CancelableReadLine(out apiParameters)) return;
                //apiParameters = Console.ReadLine();

                Console.WriteLine(
                    "Enter return type of API (press enter to skip): eg bool or List<object> or int");
                if (!CancelableReadLine(out returnType)) return;
                //returnType = Console.ReadLine();

                Console.WriteLine("======================================");

                Console.WriteLine(
                    "Enter Api service folder name:");
                if (!CancelableReadLine(out apiServiceFolderName)) return;
                //apiServiceFolderName = Console.ReadLine();

                Console.WriteLine(
                    "Enter Api service class name:");
                if (!CancelableReadLine(out apiServiceName)) return;
                //apiServiceName = Console.ReadLine();

                Console.WriteLine(
                    "Enter Api service function name:");
                if (!CancelableReadLine(out apiServiceFunctionName)) return;
                //apiServiceFunctionName = Console.ReadLine();

                Console.WriteLine("======================================");
                #endregion

                //Update Mapper files For BAL

                if (engine.CheckAPiFileExits(Constants.ControllerClassDirectory, controllerVersion, controllerName, out var controllerPath) &&
                    engine.CheckAPiServiceFileExits(Constants.ServiceDirectory, apiServiceFolderName, apiServiceName, out var servicePath, out var serviceInterfacePath))
                {
                    string apiEndPointFuncTemp = engine.ReadTemplate(TemplateType.ApiEndPointFunction);
                    string apiEndPointTemp = engine.ReadTemplate(TemplateType.ApiEndPoint);
                    string apiEndPointServiceClass = engine.ReadTemplate(TemplateType.ApiEndPointServiceClass);
                    string apiEndPointServiceInterface = engine.ReadTemplate(TemplateType.ApiEndPointServiceInterface);

                    string serviceInterface = "I" + apiServiceName + "Service";
                    string serviceVariable = (apiServiceName + "Service").ToCamelCase();
                    string routeVariable = apiRoute;

                    string apiServiceFunctionParameters = "";
                    string parametersSummary = "";
                    string apiFunctionParameters = "";
                    string apiFunctionServiceParameters = "";
                    if (!string.IsNullOrEmpty(apiParameters))
                    {
                        apiServiceFunctionParameters = ", " + apiParameters;
                        apiFunctionParameters = apiParameters;

                        List<string> allParams = apiParameters.Split(",").ToList();
                        List<string> paramNames = allParams.Select(x => x.Trim().Split(' ')[1]).ToList();

                        foreach (var paramName in paramNames)
                            parametersSummary += "\n\t\t/// <param name=\"" + paramName + "\"></param>";

                        apiFunctionServiceParameters = ", " + string.Join(", ", paramNames);
                    }

                    returnType = string.IsNullOrEmpty(returnType) ? "object" : returnType;

                    apiEndPointFuncTemp = engine.ProcessApiEndPointTemplate(apiEndPointFuncTemp, apiDescription,
                        apiRoute, apiMethodType, apiFunctionName, serviceInterface, serviceVariable,
                        apiServiceFunctionName, routeVariable, apiServiceFunctionParameters, apiFunctionParameters,
                        parametersSummary, apiFunctionServiceParameters, returnType);

                    apiEndPointTemp = engine.ProcessApiEndPointTemplate(apiEndPointTemp, apiDescription,
                        apiRoute, apiMethodType, apiFunctionName, serviceInterface, serviceVariable,
                        apiServiceFunctionName, routeVariable, apiServiceFunctionParameters, apiFunctionParameters,
                        parametersSummary, apiFunctionServiceParameters, returnType);

                    apiEndPointServiceClass = engine.ProcessApiEndPointTemplate(apiEndPointServiceClass, apiDescription,
                        apiRoute, apiMethodType, apiFunctionName, serviceInterface, serviceVariable,
                        apiServiceFunctionName, routeVariable, apiServiceFunctionParameters, apiFunctionParameters,
                        parametersSummary, apiFunctionServiceParameters, returnType);

                    apiEndPointServiceInterface = engine.ProcessApiEndPointTemplate(apiEndPointServiceInterface,
                        apiDescription,
                        apiRoute, apiMethodType, apiFunctionName, serviceInterface, serviceVariable,
                        apiServiceFunctionName, routeVariable, apiServiceFunctionParameters, apiFunctionParameters,
                        parametersSummary, apiFunctionServiceParameters, returnType);

                    string endpointControllerName = controllerName.Contains("_")
                        ? controllerName.Replace(controllerName.Substring(controllerName.IndexOf("_", StringComparison.Ordinal)), "") +
                          "Controller"
                        : controllerName;
                    engine.UpdateApiEndPointFile(Constants.ApiEndPointDirectory, apiEndPointTemp, Constants.ApiEndPointKeyword.Replace("[CONTROLLERNAME]", endpointControllerName));
                    engine.UpdateApiEndPointFile(serviceInterfacePath, apiEndPointServiceInterface);
                    engine.UpdateApiEndPointFile(servicePath, apiEndPointServiceClass);
                    engine.UpdateApiEndPointFile(controllerPath, apiEndPointFuncTemp);

                    return;
                }

                Console.WriteLine("Controller or Service not Found. Please check your input.");
            }
        }

        private static void CodeGeneratorForApiRequestParamClass(ConsoleKey selectedMode, Engine engine, string databaseCodeName)
        {
            if (selectedMode == ConsoleKey.D5 || selectedMode == ConsoleKey.NumPad5)
            {
                #region User Input
                Console.WriteLine(
                    "Enter request parameter folder name eg: WGLCS");
                if (!CancelableReadLine(out var folderName)) return;

                Console.WriteLine(
                    "Enter parameter class name eg: ParamAddDepartment");
                if (!CancelableReadLine(out var className)) return;

                Console.WriteLine("======================================");
                #endregion

                //Create ViewModel Class
                string viewModelTem = engine.ReadTemplate(TemplateType.ApiRequestParamClass);
                viewModelTem = engine.ProcessTemplate(viewModelTem, className, databaseCodeName);
                engine.WriteToFile(Constants.ApiRequestParamClassDirectory, viewModelTem, className, folderName);

                //Create Validator Class
                string validatorTem = engine.ReadTemplate(TemplateType.Validator);
                validatorTem = engine.ProcessTemplate(validatorTem, className, databaseCodeName);
                engine.WriteToFile(Constants.ValidatorClassDirectory, validatorTem, className, folderName);
            }
        }

        public static bool CancelableReadLine(out string value)
        {
            value = string.Empty;
            var buffer = new StringBuilder();
            var key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
            {
                //Console.Write(key.KeyChar);
                //buffer.Append(key.KeyChar);
                //key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Backspace && Console.CursorLeft > 0)
                {
                    var cli = --Console.CursorLeft;
                    buffer.Remove(cli, 1);
                    Console.CursorLeft = 0;
                    Console.Write(new String(System.Linq.Enumerable.Range(0, buffer.Length + 1).Select(o => ' ').ToArray()));
                    Console.CursorLeft = 0;
                    Console.Write(buffer.ToString());
                    Console.CursorLeft = cli;
                    key = Console.ReadKey(true);
                }
                else if (Char.IsLetterOrDigit(key.KeyChar) || Char.IsWhiteSpace(key.KeyChar) ||
                         Char.IsSymbol(key.KeyChar) || Char.IsPunctuation(key.KeyChar))
                {
                    var cli = Console.CursorLeft;
                    buffer.Insert(cli, key.KeyChar);
                    Console.CursorLeft = 0;
                    Console.Write(buffer.ToString());
                    Console.CursorLeft = cli + 1;
                    key = Console.ReadKey(true);
                }
                //if (key.Key == ConsoleKey.Backspace && Console.CursorLeft > 0)
                //{
                //    var cli = --Console.CursorLeft;
                //    var bufferIndex = (buffer.Length - cli) + cli - 1;
                //    buffer.Remove(bufferIndex, 1);
                //    Console.CursorLeft = cli - (buffer.Length);
                //    Console.Write(new String(System.Linq.Enumerable.Range(0, buffer.Length + 1).Select(o => ' ').ToArray()));
                //    Console.CursorLeft = cli - (buffer.Length);
                //    Console.Write(buffer.ToString());
                //    Console.CursorLeft = cli;
                //    key = Console.ReadKey(true);
                //}
                //else if (Char.IsLetterOrDigit(key.KeyChar) || Char.IsWhiteSpace(key.KeyChar) || 
                //         Char.IsSymbol(key.KeyChar) || Char.IsPunctuation(key.KeyChar))
                //{
                //    var cli = Console.CursorLeft;
                //    var bufferIndex = (buffer.Length - cli) + cli;
                //    buffer.Insert(bufferIndex, key.KeyChar);
                //    Console.CursorLeft = cli - (buffer.Length - 1);
                //    Console.Write(buffer.ToString());
                //    Console.CursorLeft = cli + 1;
                //    key = Console.ReadKey(true);
                //}
                else if (key.Key == ConsoleKey.LeftArrow && Console.CursorLeft > 0)
                {
                    Console.CursorLeft--;
                    key = Console.ReadKey(true);
                }
                else if (key.Key == ConsoleKey.RightArrow && Console.CursorLeft < buffer.Length)
                {
                    Console.CursorLeft++;
                    key = Console.ReadKey(true);
                }
                else
                {
                    key = Console.ReadKey(true);
                }
            }

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                value = buffer.ToString();
                return true;
            }
            return false;
        }
    }
}
