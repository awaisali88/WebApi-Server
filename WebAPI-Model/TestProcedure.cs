using System.Data;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Extensions;

namespace WebAPI_Model
{
    public class TestProcedure : ISProcParam
    {
        public string ProcedureName => "";

        [ProcedureParam("", DbType.Int32)]
        public int ParamValue { get; set; }
    }
}

