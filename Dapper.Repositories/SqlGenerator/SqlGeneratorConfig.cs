using Common;

namespace Dapper.Repositories.SqlGenerator
{
    /// <summary>
    ///     Config for SqlGenerator
    /// </summary>
    public class SqlGeneratorConfig
    {
        /// <summary>
        ///     Type Sql provider
        /// </summary>
        public SqlProvider SqlProvider { get; set; }

        /// <summary>
        ///     Use quotation marks for TableName and ColumnName
        /// </summary>
        public bool UseQuotationMarks { get; set; }

        /// <summary>
        ///     Use quotation marks for TableName and ColumnName
        /// </summary>
        public bool LogQuery { get; set; }
    }
}