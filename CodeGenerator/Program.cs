﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common;
using Microsoft.Extensions.DependencyInjection;
using WebAPI_BAL;
using WebAPI_DataAccess.WebApiContext;
using WebAPI_DataAccess.NorthwindContext;

namespace CodeGenerator
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                //while (true)
                //{
                //    var key = Console.ReadKey(true).Key;
                //    Console.WriteLine(key.ToString());
                //}

                var serviceProvider = InitApplication.InitApp();
                bool isContinue = true;
                bool isContinueForDatabase = true;
                bool isContinueForMode = true;
                bool isContinueForTable = true;

                //string dirPath = @"D:\My_Data\Projects\LiveAdmins\WGApi\wgapi\CodeGenerator\";
                string dirPath = @"";
                bool isContinueDirPath = true;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Please provide full directory path of CodeGenerator project solution");
                    Console.WriteLine(@"eg: D:\My_Data\Projects\NLT\Github\WebApi-Server\");
                    Console.WriteLine();
                    Console.WriteLine("Enter Directory Path:");
                    dirPath = Console.ReadLine();
                    if (!string.IsNullOrEmpty(dirPath))
                    {
                        Console.Clear();
                        isContinueDirPath = false;
                    }
                    else
                        Console.Clear();
                } while (isContinueDirPath);

                while (isContinue)
                {
                    Console.Clear();
                    Engine engine = new Engine(dirPath);

                    ConsoleKey selectedDatabase;
                    do
                    {
                        Console.WriteLine("Available databases:\n1. WebApiDb\n2. Northwind");
                        Console.WriteLine();
                        Console.Write("Please select database:\t");

                        selectedDatabase = Console.ReadKey(true).Key;
                        if (selectedDatabase == ConsoleKey.D1 || selectedDatabase == ConsoleKey.D2 ||
                            selectedDatabase == ConsoleKey.NumPad1 || selectedDatabase == ConsoleKey.NumPad2)
                        {
                            isContinueForDatabase = false;
                            Console.WriteLine(SelectDatabase(selectedDatabase));
                            Console.WriteLine("======================================");
                        }
                        else
                            Console.Clear();
                    } while (isContinueForDatabase);

                    string databaseName = SelectDatabase(selectedDatabase);
                    if (databaseName == "") break;

                    string databaseCodeName =
                        databaseName == engine.WebApiDbDatabaseName ? engine.WebApiDbCodeName : databaseName == engine.NorthwindDatabaseName ? engine.NorthwindCodeName : databaseName;

                    ConsoleKey selectedMode;
                    do
                    {
                        Console.WriteLine("Available Generators:\n1. Table\n2. Store Procedure\n3. Api Service");
                        Console.WriteLine();
                        Console.Write("Please select mode:\t");

                        selectedMode = Console.ReadKey(true).Key;
                        if (selectedMode == ConsoleKey.D1 || selectedMode == ConsoleKey.D2 ||
                            selectedMode == ConsoleKey.NumPad1 || selectedMode == ConsoleKey.NumPad2 ||
                            selectedMode == ConsoleKey.D3 || selectedMode == ConsoleKey.NumPad3)
                        {
                            isContinueForMode = false;
                            Console.WriteLine(SelectMode(selectedMode));
                            Console.WriteLine("======================================");
                        }
                        else
                            Console.Clear();
                    } while (isContinueForMode);

                    if (selectedMode == ConsoleKey.D1 || selectedMode == ConsoleKey.NumPad1)
                    {
                        string fullTableName = "";
                        do
                        {
                            Console.WriteLine(
                                "Enter fully qualified table name eg: Schema.TableName (dbo.Customers)");

                            fullTableName = Console.ReadLine();
                            Console.WriteLine("======================================");
                            if (fullTableName != null && fullTableName.Split('.').Length == 2)
                            {
                                isContinueForTable = false;
                            }
                        } while (isContinueForTable);

                        string schemaName = fullTableName.Split('.')[0];
                        string tableName, model, viewModel;
                        tableName = model = fullTableName.Split('.')[1];

                        Console.WriteLine("Generating Model Class Code");
                        string generatedModelClass = engine.GetGeneratedModelClass(databaseName,
                            $"{schemaName}.{tableName}", serviceProvider);
                        Console.WriteLine("Generating ViewModel Class Code");
                        string generatedViewModelClass = engine.GetGeneratedViewModelClass(databaseName,
                            $"{schemaName}.{tableName}", serviceProvider);

                        Console.WriteLine("Generating files");
                        //Create Model Class
                        string modelTem = engine.ReadTemplate(TemplateType.Model);
                        modelTem = engine.ProcessTemplate(modelTem, model, databaseCodeName, generatedModelClass,
                            generatedViewModelClass);
                        bool r = engine.WriteToFile(Constants.ModelClassDirectory, modelTem, model, databaseName);
                        if (r)
                        {

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
                            balClassTem = engine.ProcessTemplate(balClassTem, model, databaseCodeName,
                                generatedModelClass,
                                generatedViewModelClass);
                            engine.WriteToFile(Constants.BalClassDirectory, balClassTem, model, databaseName);

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
                            if (databaseName == engine.WebApiDbDatabaseName)
                            {
                                engine.UpdateToFile(Constants.WebApiDbDbContextInterfaceDirectory, repoInterface, model,
                                    databaseName, repoKeyword, initialSpaces);
                                engine.UpdateToFile(Constants.AppContextPublicClassDirectory, repoPublic, model,
                                    databaseName, repoKeyword, initialSpaces);
                                engine.UpdateToFile(Constants.AppContextPrivateClassDirectory, repoPrivate, model,
                                    databaseName, repoKeyword, initialSpaces);
                            }

                            if (databaseName == engine.NorthwindDatabaseName)
                            {
                                engine.UpdateToFile(Constants.NorthwindDbContextInterfaceDirectory, repoInterface,
                                    model,
                                    databaseName, repoKeyword, initialSpaces);
                                engine.UpdateToFile(Constants.TicketSystemContextPublicClassDirectory, repoPublic,
                                    model,
                                    databaseName, repoKeyword, initialSpaces);
                                engine.UpdateToFile(Constants.TicketSystemContextPrivateClassDirectory, repoPrivate,
                                    model,
                                    databaseName, repoKeyword, initialSpaces);
                            }

                            //Update Mapper files
                            string mapperClassTemp = engine.ReadTemplate(TemplateType.Mapper);
                            mapperClassTemp = engine.ProcessTemplate(mapperClassTemp, model, databaseCodeName,
                                generatedModelClass,
                                generatedViewModelClass);

                            string mapperCode = mapperClassTemp.Split('\n')[0]
                                .Substring(0, mapperClassTemp.Split('\n')[0].Length - 1);
                            string diCode = mapperClassTemp.Split('\n')[1]
                                .Substring(0, mapperClassTemp.Split('\n')[1].Length - 1);

                            string mapperKeyword =
                                Constants.MappingProfileKeyword.Replace("[DATABASENAME]", databaseName);
                            string diKeyword = Constants.RegisterServiceKeyword.Replace("[DATABASENAME]", databaseName);
                            string mapperSpaces = "\t\t\t";
                            engine.UpdateToFile(Constants.MappingProfileClassDirectory, mapperCode, model, databaseName,
                                mapperKeyword, mapperSpaces);
                            engine.UpdateToFile(Constants.RegisterServiceClassDirectory, diCode, model, databaseName,
                                diKeyword, mapperSpaces);
                        }
                    }

                    if (selectedMode == ConsoleKey.D2 || selectedMode == ConsoleKey.NumPad2)
                    {
                        Console.WriteLine("Store Procedure Generator");
                    }

                    if (selectedMode == ConsoleKey.D3 || selectedMode == ConsoleKey.NumPad3)
                    {
                        Console.Write("Enter folder name:\t");
                        string apiServiceFolderName = Console.ReadLine();
                        Console.Write("Enter Api service name:\t");
                        string apiServiceName = Console.ReadLine();
                        Console.Write("Enter  one or multiple BAL Interfaces eg. IUserBal,ICustomerBal,...:\t");
                        string balClasses = Console.ReadLine();
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
                        bool r = engine.WriteToFile(Constants.ApiServiceInterfaceDirectory, serviceInterfaceTem, "", databaseName, apiServiceFolderName, apiServiceName);

                        if (r)
                        {
                            //Create Api Service Class
                            string serviceTem = engine.ReadTemplate(TemplateType.ApiService);
                            serviceTem = engine.ProcessApiServiceTemplate(serviceTem, apiServiceFolderName,
                                apiServiceName,
                                balInit, balCtor, balLink);
                            engine.WriteToFile(Constants.ApiServiceDirectory, serviceTem, "", databaseName,
                                apiServiceFolderName, apiServiceName);

                            //Update Mapper files
                            string mapperClassTemp = engine.ReadTemplate(TemplateType.Mapper);
                            mapperClassTemp = engine.ProcessApiServiceTemplate(mapperClassTemp, apiServiceFolderName,
                                apiServiceName,
                                balInit, balCtor, balLink);

                            string serviceCode = mapperClassTemp.Split('\n')[2]
                                .Substring(0, mapperClassTemp.Split('\n')[2].Length - 1);

                            string serviceKeyword = Constants.RegisterApiServiceKeyword;
                            string mapperSpaces = "\t\t\t";
                            engine.UpdateToFile(Constants.RegisterServiceClassDirectory, serviceCode, "", databaseName,
                                serviceKeyword, mapperSpaces);
                        }
                    }

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

        private static string SelectDatabase(ConsoleKey pressedKey)
        {
            Engine e = new Engine("");
            return pressedKey == ConsoleKey.D1 || pressedKey == ConsoleKey.NumPad1
                ? e.WebApiDbDatabaseName
                : pressedKey == ConsoleKey.D2 || pressedKey == ConsoleKey.NumPad2
                    ? e.NorthwindDatabaseName
                    : "";
        }

        private static string SelectMode(ConsoleKey pressedKey)
        {
            return pressedKey == ConsoleKey.D1 || pressedKey == ConsoleKey.NumPad1
                ? "Table"
                : pressedKey == ConsoleKey.D2 || pressedKey == ConsoleKey.NumPad2
                    ? "StoreProcedure"
                    : pressedKey == ConsoleKey.D3 || pressedKey == ConsoleKey.NumPad3
                        ? "Api Service"
                    : "";
        }
    }
}