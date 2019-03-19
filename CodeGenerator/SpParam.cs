using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Extensions;

namespace CodeGenerator
{
    public class GetModelClassParam : ISProcParam
    {
        public string ProcedureName => "CreateC#ModelFromTable";

        [ProcedureParam("@TableName", typeof(string))]
        public string TableName { get; set; }
    }

    public class GetModelClassParamViewModel
    {
        public string TableName { get; set; }
    }

    public class GetViewModelClassParam : ISProcParam
    {
        public string ProcedureName => "CreateC#ViewModelFromTable";

        [ProcedureParam("@TableName", typeof(string))]
        public string TableName { get; set; }
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
        public string ProcedureName => "dbo.CreateC#SpParamModel";

        [ProcedureParam("@spFullName", typeof(string))]
        public string SpName { get; set; }

        [ProcedureParam("@modelName", typeof(string))]
        public string ModelName { get; set; }
    }

    public class GetSpParamViewModelClassParam : ISProcParam
    {
        public string ProcedureName => "dbo.CreateC#SpParamViewModel";

        [ProcedureParam("@spFullName", typeof(string))]
        public string SpName { get; set; }

        [ProcedureParam("@modelName", typeof(string))]
        public string ModelName { get; set; }
    }

    public class GetSpReturnModelClassParam : ISProcParam
    {
        public string ProcedureName => "dbo.CreateC#SpReturnModel";

        [ProcedureParam("@spFullName", typeof(string))]
        public string SpName { get; set; }

        [ProcedureParam("@modelName", typeof(string))]
        public string ModelName { get; set; }
    }

    public class GetSpReturnViewModelClassParam : ISProcParam
    {
        public string ProcedureName => "dbo.CreateC#SpReturnViewModel";

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
