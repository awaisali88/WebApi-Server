using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Extensions;

namespace CodeGenerator
{
    public static class ProcedureNames
    {
        public const string CreateModelFromTable = "dbo.CreateC#ModelFromTable";
        public const string CreateViewModelFromTable = "dbo.CreateC#ViewModelFromTable";
        public const string CreateSpParamModel = "dbo.CreateC#SpParamModel";
        public const string CreateSpParamViewModel = "dbo.CreateC#SpParamViewModel";
        public const string CreateSpReturnModel = "dbo.CreateC#SpReturnModel";
        public const string CreateSpReturnViewModel = "dbo.CreateC#SpReturnViewModel";
    }

    public class GetModelClassParam : ISProcParam
    {
        public string ProcedureName => ProcedureNames.CreateModelFromTable;

        [ProcedureParam("@TableName", typeof(string))]
        public string TableName { get; set; }

        [ProcedureParam("@ModelName", typeof(string))]
        public string ModelName { get; set; }
    }

    public class GetViewModelClassParam : ISProcParam
    {
        public string ProcedureName => ProcedureNames.CreateViewModelFromTable;

        [ProcedureParam("@TableName", typeof(string))]
        public string TableName { get; set; }

        [ProcedureParam("@ModelName", typeof(string))]
        public string ModelName { get; set; }
    }

    public class GetModelClassParamViewModel
    {
        public string TableName { get; set; }

        public string ModelName { get; set; }
    }

    public class GeneratedModelClass
    {
        public string ModelClass { get; set; }
    }

    public class GeneratedModelClassViewModel
    {
        public string ModelClass { get; set; }
    }

    ////////////////////////////////////////////////////////
    // ReSharper disable once InvalidXmlDocComment
    /// Generate Store procedure Classes
    ////////////////////////////////////////////////////////

    public class GetSpParamModelClassParam : ISProcParam
    {
        public string ProcedureName => ProcedureNames.CreateSpParamModel;

        [ProcedureParam("@spFullName", typeof(string))]
        public string SpName { get; set; }

        [ProcedureParam("@modelName", typeof(string))]
        public string ModelName { get; set; }
    }

    public class GetSpParamViewModelClassParam : ISProcParam
    {
        public string ProcedureName => ProcedureNames.CreateSpParamViewModel;

        [ProcedureParam("@spFullName", typeof(string))]
        public string SpName { get; set; }

        [ProcedureParam("@modelName", typeof(string))]
        public string ModelName { get; set; }
    }

    public class GetSpReturnModelClassParam : ISProcParam
    {
        public string ProcedureName => ProcedureNames.CreateSpReturnModel;

        [ProcedureParam("@spFullName", typeof(string))]
        public string SpName { get; set; }

        [ProcedureParam("@modelName", typeof(string))]
        public string ModelName { get; set; }
    }

    public class GetSpReturnViewModelClassParam : ISProcParam
    {
        public string ProcedureName => ProcedureNames.CreateSpReturnViewModel;

        [ProcedureParam("@spFullName", typeof(string))]
        public string SpName { get; set; }

        [ProcedureParam("@modelName", typeof(string))]
        public string ModelName { get; set; }
    }

    public class GetSpModelClassParamViewModel
    {
        public string SpName { get; set; }
        public string ModelName { get; set; }
    }
}