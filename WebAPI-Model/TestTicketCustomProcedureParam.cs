using System.Data;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Extensions;

namespace WebAPI_Model
{
    public class TestTicketCustomProcedureParam : ISProcParam
    {
        public string ProcedureName => "PROC_Ticket_Custom_Search_V3";

        [ProcedureParam("@TicketDepartmentID", DbType.Decimal)]
        public long TicketDepartmentId { get; set; }

        [ProcedureParam("@UserID", DbType.Decimal)]
        public long UserId { get; set; }

        [ProcedureParam("@EmpCompanyId", DbType.String)]
        public string EmpCompanyId { get; set; }

        [ProcedureParam("@TicketUserName", DbType.String)]
        public string TicketUserName { get; set; }

        [ProcedureParam("@TicketStatusID", DbType.String)]
        public string TicketStatusId { get; set; }
    }
}

