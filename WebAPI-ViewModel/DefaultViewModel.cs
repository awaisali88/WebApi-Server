using System;
using Common;
using Common.JsonConverter;
using Newtonsoft.Json;

namespace WebAPI_ViewModel
{
    public abstract class DefaultViewModel
    {
        public virtual bool Status { get; set; }// = true;
        public virtual bool Trashed { get; set; }// = false;

        public byte[] RowVersion { get; set; }

        public virtual DateTime? CreatedDate { get; set; }// = DateTime.UtcNow;

        public virtual DateTime? ModifiedDate { get; set; }// = DateTime.UtcNow;

        public virtual string CreatedBy { get; set; }

        public virtual string ModifiedBy { get; set; }

        [JsonIgnore]
        public virtual int RecordStatusCode
        {
            get => RecordStatus.EnumValue();
            set => RecordStatus = value.EnumValue<RecordStatus>();
        }

        [JsonConverter(typeof(StringValueEnumConverter<RecordStatus>))]
        public virtual RecordStatus RecordStatus { get; set; }// = RecordStatus.NewMode;
    }
}
