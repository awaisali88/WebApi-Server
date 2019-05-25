using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator
{
    public static class Constants
    {
        public const string BalClassDirectory = @"WebAPI-BAL\BLL\[DATABASENAME]\[MODEL]Bal.cs";
        public const string BalInterfaceDirectory = @"WebAPI-BAL\BLL\[DATABASENAME]\Interfaces\I[MODEL]Bal.cs";

        public const string ModelClassDirectory = @"WebAPI-Model\[DATABASENAME]\[MODEL]Model.cs";
        public const string ViewModelClassDirectory = @"WebAPI-ViewModel\DTO\[DATABASENAME]\[MODEL]ViewModel.cs";
        public const string ValidatorClassDirectory = @"WebAPI-ViewModel\Validator\[DATABASENAME]\[MODEL]ViewModelValidator.cs";

        public const string MappingProfileClassDirectory = @"WebAPI-Server\AppStart\MappingProfile.cs";
        public const string MappingProfileKeyword = @"//[AUTO_GENERATED_MAPPER_[DATABASENAME]]";

        public const string RegisterServiceClassDirectory = @"WebAPI-Server\AppStart\RegisterServices\ScopedServices.cs";
        public const string RegisterServiceKeyword = @"//[AUTO_GENERATED_SCOPED_SERVICES_[DATABASENAME]]";
        public const string RegisterApiServiceKeyword = @"//[AUTO_GENERATED_SCOPED_ApiServices]";

        #region DbContext Directories
        //WebApiDb Context
        public const string Db1DbContextInterfaceDirectory = @"WebAPI-DataAccess\WebApiContext\IWebApiDbContext.cs";
        public const string Db1PublicClassDirectory = @"WebAPI-DataAccess\WebApiContext\WebApiDbContext_Public.cs";
        public const string Db1PrivateClassDirectory = @"WebAPI-DataAccess\WebApiContext\WebApiDbContext_Private.cs";
        
        //Northwind Context
        public const string Db2DbContextInterfaceDirectory = @"WebAPI-DataAccess\NorthwindContext\INorthwindDbContext.cs";
        public const string Db2PublicClassDirectory = @"WebAPI-DataAccess\NorthwindContext\NorthwindDbContext_Public.cs";
        public const string Db2PrivateClassDirectory = @"WebAPI-DataAccess\NorthwindContext\NorthwindDbContext_Private.cs";

        //Db3 Context
        public const string Db3DbContextInterfaceDirectory = @"";
        public const string Db3PublicClassDirectory = @"";
        public const string Db3PrivateClassDirectory = @"";

        //Db4 Context
        public const string Db4DbContextInterfaceDirectory = @"";
        public const string Db4PublicClassDirectory = @"";
        public const string Db4PrivateClassDirectory = @"";

        //Db5 Context
        public const string Db5DbContextInterfaceDirectory = @"";
        public const string Db5PublicClassDirectory = @"";
        public const string Db5PrivateClassDirectory = @"";

        //Db6 Context
        public const string Db6DbContextInterfaceDirectory = @"";
        public const string Db6PublicClassDirectory = @"";
        public const string Db6PrivateClassDirectory = @"";

        //Db7 Context
        public const string Db7DbContextInterfaceDirectory = @"";
        public const string Db7PublicClassDirectory = @"";
        public const string Db7PrivateClassDirectory = @"";

        //Db8 Context
        public const string Db8DbContextInterfaceDirectory = @"";
        public const string Db8PublicClassDirectory = @"";
        public const string Db8PrivateClassDirectory = @"";

        //Db9 Context
        public const string Db9DbContextInterfaceDirectory = @"";
        public const string Db9PublicClassDirectory = @"";
        public const string Db9PrivateClassDirectory = @"";
        #endregion

        public const string RepositoryKeyword = @"//[AUTO_GENERATED_REPO_[DATABASENAME]]";

        public const string ApiServiceInterfaceDirectory = @"WebAPI-Service\[SERVICEDIR]\Interfaces\I[SERVICENAME]Service.cs";
        public const string ApiServiceDirectory = @"WebAPI-Service\[SERVICEDIR]\[SERVICENAME]Service.cs";

        public const string SpParamModelDirectory = @"WebAPI-Model\[DATABASENAME]_Proc\Parameters\[MODEL]Param.cs";
        public const string SpParamViewModelDirectory = @"WebAPI-ViewModel\DTO\[DATABASENAME]_Proc\Parameters\[MODEL]ParamViewModel.cs";
        public const string SpReturnModelDirectory = @"WebAPI-Model\[DATABASENAME]_Proc\[MODEL]Model.cs";
        public const string SpReturnViewModelDirectory = @"WebAPI-ViewModel\DTO\[DATABASENAME]_Proc\[MODEL]ViewModel.cs";
        public const string SpParamMappingProfileKeyword = @"//[AUTO_GENERATED_SPPARAM_MAPPER_[DATABASENAME]]";
        public const string SpReturnMappingProfileKeyword = @"//[AUTO_GENERATED_SPRETURN_MAPPER_[DATABASENAME]]";

        public const string SpProcNameDirectory = @"WebAPI-Model\StoreProcedureNames.cs";
        public const string SpProcNameKeyword = @"//[AUTO_GENERATED_SPROCNAME_[DATABASENAME]]";

        public const string ControllerClassDirectory = @"WebAPI-Server\Controllers\[VERSIONDIR]";
        public const string ServiceDirectory = @"WebAPI-Service\[SERVICEDIRNAME]";
        public const string ApiEndPointDirectory = @"WebAPI-Server\Controllers\ApiEndpoints.cs";
        public const string ApiEndPointKeyword = @"//[[CONTROLLERNAME]_Do_Not_Remove_This_Line]";

        #region Templates
        public const string ModelTemplate = @"CodeGenerator\Templates\Model";
        public const string ViewModelTemplate = @"CodeGenerator\Templates\ViewModel";
        public const string BalTemplate = @"CodeGenerator\Templates\Bal";
        public const string IBalTemplate = @"CodeGenerator\Templates\IBal";
        public const string MapperTemplate = @"CodeGenerator\Templates\Mapper";
        public const string RepositoryTemplate = @"CodeGenerator\Templates\Repository";
        public const string ValidatorTemplate = @"CodeGenerator\Templates\Validator";
        public const string ApiServiceTemplate = @"CodeGenerator\Templates\ApiService";
        public const string IApiServiceTemplate = @"CodeGenerator\Templates\IApiService";

        public const string SpParamModelTemplate = @"CodeGenerator\Templates\SpParamModel";
        public const string SpParamViewModelTemplate = @"CodeGenerator\Templates\SpParamViewModel";
        public const string SpReturnModelTemplate = @"CodeGenerator\Templates\SpReturnModel";
        public const string SpReturnViewModelTemplate = @"CodeGenerator\Templates\SpReturnViewModel";

        public const string ApiEndPointFunctionTemplate = @"CodeGenerator\Templates\ApiEndPointFunction";
        public const string ApiEndPointTemplate = @"CodeGenerator\Templates\ApiEndPoint";
        public const string ApiEndPointServiceClassTemplate = @"CodeGenerator\Templates\ApiEndPointServiceClass";
        public const string ApiEndPointServiceInterfaceTemplate = @"CodeGenerator\Templates\ApiEndPointServiceInterface";
        #endregion

        #region TemplateKeywords
        public const string ModelTemKeyword = @"\[MODEL\]";
        public const string DatabaseTemKeyword = @"\[DATABASE\]";
        public const string ModelCcTemKeyword = @"\[MODELCC\]";
        public const string ModelClassTemKeyword = @"\[MODELCLASS\]";
        public const string ViewModelClassTemKeyword = @"\[VIEWMODELCLASS\]";
        public const string ApiServiceKeywordSDir = @"\[SERVICEDIR\]";
        public const string ApiServiceKeywordSName = @"\[SERVICENAME\]";
        // ReSharper disable once InconsistentNaming
        public const string ApiServiceKeywordSBInit = @"\[BALDIINIT\]";
        // ReSharper disable once InconsistentNaming
        public const string ApiServiceKeywordSBCtor = @"\[BALDICTOR\]";
        // ReSharper disable once InconsistentNaming
        public const string ApiServiceKeywordSBLink = @"\[BALDILINK\]";

        public const string ApiDescription = @"\[APIDESCRIPTION\]";
        public const string ApiEndPointName = @"\[APIENDPOINTNAME\]";
        public const string ApiEndPointNameVariable = @"\[APIENDPOINTNAMEVARIABLE\]";
        public const string ApiHttpMethod = @"\[METHODTYPE\]";
        public const string ApiEndPointFunctionName = @"\[APIFUNCTIONNAME\]";
        public const string ApiServiceInterface = @"\[APISERVICEINTERFACE\]";
        public const string ApiServiceVariable = @"\[APISERVICEVARIABLE\]";
        public const string ApiServiceFunction = @"\[APISERVICEFUNCTION\]";

        public const string ApiServiceFunctionParameters = @"\[APISERVICEFUNCTIONPARAMETERS\]";
        public const string ParametersSummary = @"\[PARAMETERSSUMMARY\]";
        public const string ApiFunctionParameters = @"\[APIFUNCTIONPARAMETERS\]";
        public const string ApiFunctionServiceParameters = @"\[APIFUNCTIONSERVICEPARAMETERS\]";
        public const string ReturnType = @"\[RETURNTYPE\]";
        #endregion
    }

    public enum TemplateType
    {
        Bal,
        IBal,
        Mapper,
        Model,
        ViewModel,
        Repository,
        Validator,
        ApiService,
        IApiService,

        SpParamModel,
        SpParamViewModel,
        SpReturnModel,
        SpReturnViewModel,

        ApiEndPoint,
        ApiEndPointFunction,
        ApiEndPointServiceClass,
        ApiEndPointServiceInterface
    }
}
