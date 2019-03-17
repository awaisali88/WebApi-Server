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

        [ProcedureParam("@TableName", DbType.String)]
        public string TableName { get; set; }
    }

    public class GetModelClassParamViewModel
    {
        public string TableName { get; set; }
    }

    public class GetViewModelClassParam : ISProcParam
    {
        public string ProcedureName => "CreateC#ViewModelFromTable";

        [ProcedureParam("@TableName", DbType.String)]
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
}
