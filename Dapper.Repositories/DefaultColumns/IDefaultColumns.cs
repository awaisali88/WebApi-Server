using System;
using Common;

namespace Dapper.Repositories
{
    public interface IDefaultColumns
    {
        bool Status { get; set; }
        bool Trashed { get; set; }

        byte[] RowVersion { get; set; }

        DateTime? CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }

        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }

        int RecordStatusCode { get; set; }
        RecordStatus RecordStatus { get; set; }
    }
}
