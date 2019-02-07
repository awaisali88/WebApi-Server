using System;

namespace Common
{
    public interface IDefaultColumns
    {
        bool Status { get; set; }
        bool Trashed { get; set; }

        byte[] RowVersion { get; set; }

        DateTime CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }

        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }

        int RecordStatusCode { get; set; }
    }
}
