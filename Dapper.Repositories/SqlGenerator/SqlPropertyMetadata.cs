using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Dapper.Repositories.Attributes;

namespace Dapper.Repositories.SqlGenerator
{
    /// <summary>
    ///     Metadata from PropertyInfo
    /// </summary>
    public class SqlPropertyMetadata
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public SqlPropertyMetadata(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            var alias = PropertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (!string.IsNullOrEmpty(alias?.Name))
            {
                Alias = alias.Name;
                ColumnName = Alias;
            }
            else
            {
                ColumnName = PropertyInfo.Name;
            }
            var ignoreUpdate = PropertyInfo.GetCustomAttribute<IgnoreUpdateAttribute>();
            if (ignoreUpdate != null)
                IgnoreUpdate = true;

            var ignoreCreate = PropertyInfo.GetCustomAttribute<IgnoreCreateAttribute>();
            if (ignoreCreate != null)
                IgnoreCreate = true;

            var rowVersion = PropertyInfo.GetCustomAttribute<RowVersionAttribute>();
            if (rowVersion != null)
                RowVersionProp = true;
        }

        /// <summary>
        ///     Original PropertyInfo
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        ///     Alias for ColumnName
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        ///     ColumnName
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     Exclude property from Create
        /// </summary>
        public bool IgnoreCreate { get; set; }

        /// <summary>
        ///     Exclude property from update
        /// </summary>
        public bool IgnoreUpdate { get; set; }

        /// <summary>
        ///     Exclude property from update
        /// </summary>
        public bool RowVersionProp { get; set; }

        /// <summary>
        ///     PropertyName
        /// </summary>
        public virtual string PropertyName => PropertyInfo.Name;
    }
}