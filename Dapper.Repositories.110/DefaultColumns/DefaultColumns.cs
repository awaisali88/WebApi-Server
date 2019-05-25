using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Common;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Attributes.LogicalDelete;

namespace Dapper.Repositories
{
    public abstract class DefaultColumns : IDefaultColumns
    {
        [Column("Status")]
        public virtual bool Status { get; set; }

        [Status, Deleted]
        [Column("Trashed")]
        public virtual bool Trashed { get; set; }

        [Timestamp]
        [ConcurrencyCheck]
        [RowVersion]
        [Column("RowVersion")]
        public virtual byte[] RowVersion { get; set; }

        [CreatedAt]
        [IgnoreUpdate]
        [Column("CreatedDate")]
        public virtual DateTime? CreatedDate { get; set; }

        [MandatoryUpdate]
        [UpdatedAt]
        [Column("ModifiedDate")]
        public virtual DateTime? ModifiedDate { get; set; }

        [IgnoreUpdate]
        [Column("CreatedBy")]
        public virtual string CreatedBy { get; set; }

        [MandatoryUpdate]
        [Column("ModifiedBy")]
        public virtual string ModifiedBy { get; set; }

        [MandatoryUpdate]
        [Column("RecordStatus")]
        public virtual int RecordStatusCode
        {
            get => RecordStatus.EnumValue();
            set => RecordStatus = value.EnumValue<RecordStatus>();
        }

        [NotMapped]
        public virtual RecordStatus RecordStatus { get; set; }

        public static string TableName()
        {
            Type classType = MethodBase.GetCurrentMethod().DeclaringType;
            var tableAttribute = classType.GetCustomAttributes(typeof(TableAttribute), true);
            if (tableAttribute != null && tableAttribute.Any())
                return ((TableAttribute)tableAttribute.FirstOrDefault())?.Name;

            return classType.Name;
        }

        public static string ColumnName(string propertyName)
        {
            Type classType = MethodBase.GetCurrentMethod().DeclaringType;
            PropertyInfo property = classType.GetProperty(propertyName);
            if (property != null)
            {
                var columnAttribute = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (columnAttribute != null && columnAttribute.Any())
                    return ((ColumnAttribute)columnAttribute.FirstOrDefault())?.Name;
            }
            return propertyName;
        }

        public string GetTableName()
        {
            var tableAttribute = GetType().GetCustomAttributes(typeof(TableAttribute), true);
            if (tableAttribute != null && tableAttribute.Any())
                return ((TableAttribute)tableAttribute.FirstOrDefault())?.Name;

            return GetType().Name;
        }

        public string GetColumnName(string propertyName)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            if (property != null)
            {
                var columnAttribute = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (columnAttribute != null && columnAttribute.Any())
                    return ((ColumnAttribute)columnAttribute.FirstOrDefault())?.Name;
            }
            return propertyName;
        }
    }
}
