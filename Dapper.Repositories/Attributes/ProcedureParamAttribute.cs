using System;
using System.Data;

namespace Dapper.Repositories.Attributes
{
    /// <inheritdoc />
    /// <summary>
    ///     UpdatedAt. Warning!!! Changes the property during SQL generation
    /// </summary>
    public sealed class ProcedureParamAttribute : Attribute
    {
        public DbType SelectedDbType { get; set; }

        public ParameterDirection ParameterDirection { get; set; }

        public string ParameterName { get; set; }

        public ProcedureParamAttribute(string name, DbType type)
        {
            ParameterName = name;
            SelectedDbType = type;
            ParameterDirection = ParameterDirection.Input;
        }
    }
}