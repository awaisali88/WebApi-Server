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

        //GreeterApp Context
        public const string WebApiDbDbContextInterfaceDirectory = @"WebAPI-DataAccess\WebApiContext\IWebApiDbContext.cs";
        public const string AppContextPublicClassDirectory = @"WebAPI-DataAccess\WebApiContext\WebApiDbContext_Public.cs";
        public const string AppContextPrivateClassDirectory = @"WebAPI-DataAccess\WebApiContext\WebApiDbContext_Private.cs";
        //TicketSystem Context
        public const string NorthwindDbContextInterfaceDirectory = @"WebAPI-DataAccess\NorthwindContext\INorthwindDbContext.cs";
        public const string TicketSystemContextPublicClassDirectory = @"WebAPI-DataAccess\NorthwindContext\NorthwindDbContext_Public.cs";
        public const string TicketSystemContextPrivateClassDirectory = @"WebAPI-DataAccess\NorthwindContext\NorthwindDbContext_Private.cs";
        public const string RepositoryKeyword = @"//[AUTO_GENERATED_REPO_[DATABASENAME]]";

        public const string ApiServiceInterfaceDirectory = @"WebAPI-Service\[SERVICEDIR]\Interfaces\I[SERVICENAME]Service.cs";
        public const string ApiServiceDirectory = @"WebAPI-Service\[SERVICEDIR]\[SERVICENAME]Service.cs";

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
        #endregion

        #region TemplateKeywords
        public const string ModelTemKeyword = @"\[MODEL\]";
        public const string DatabaseTemKeyword = @"\[DATABASE\]";
        public const string ModelCcTemKeyword = @"\[MODELCC\]";
        public const string ModelClassTemKeyword = @"\[MODELCLASS\]";
        public const string ViewModelClassTemKeyword = @"\[VIEWMODELCLASS\]";
        public const string ApiServiceKeywordSDIR = @"\[SERVICEDIR\]";
        public const string ApiServiceKeywordSNAME = @"\[SERVICENAME\]";
        public const string ApiServiceKeywordSBINIT = @"\[BALDIINIT\]";
        public const string ApiServiceKeywordSBCTOR = @"\[BALDICTOR\]";
        public const string ApiServiceKeywordSBLINK = @"\[BALDILINK\]";
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
        IApiService
    }
}
