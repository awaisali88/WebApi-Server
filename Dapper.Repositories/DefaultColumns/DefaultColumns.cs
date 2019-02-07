using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual byte[] RowVersion { get; set; }

        [CreatedAt]
        [IgnoreUpdate]
        [Column("CreatedDate")]
        public virtual DateTime? CreatedDate { get; set; }

        [UpdatedAt]
        [Column("ModifiedDate")]
        public virtual DateTime? ModifiedDate { get; set; }

        [IgnoreUpdate]
        [Column("CreatedBy")]
        public virtual string CreatedBy { get; set; }

        [Column("ModifiedBy")]
        public virtual string ModifiedBy { get; set; }

        [Column("RecordStatus")]
        public virtual int RecordStatusCode
        {
            get => RecordStatus.EnumValue();
            set => RecordStatus = value.EnumValue<RecordStatus>();
        }

        [NotMapped]
        public virtual RecordStatus RecordStatus { get; set; }
    }
}
