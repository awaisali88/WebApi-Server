using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common
{
    public abstract class DefaultColumns : IDefaultColumns
    {
        private DateTime? _createdDate;
        private DateTime? _modifiedDate;

        [IsActive]
        [Column("Status")]
        public virtual bool Status { get; set; }

        [IsActive]
        [Column("Trashed")]
        public virtual bool Trashed { get; set; }

        [Timestamp]
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }

        [DateCreated]
        [Column("CreatedDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime CreatedDate
        {
            get => _createdDate ?? DateTime.UtcNow;
            set => _createdDate = value;
        }

        [DateModified]
        [Column("ModifiedDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? ModifiedDate
        {
            get => _modifiedDate ?? DateTime.UtcNow;
            set => _modifiedDate = value;
        }

        [CreatedBy]
        [Column("CreatedBy")]
        public virtual string CreatedBy { get; set; }

        [ModifiedBy]
        [Column("ModifiedBy")]
        public virtual string ModifiedBy { get; set; }

        [RecordStatus]
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
