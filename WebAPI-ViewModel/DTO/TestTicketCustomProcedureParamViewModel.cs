using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI_ViewModel.DTO
{
    public class TestTicketCustomProcedureParamViewModel
    {
        public string TicketStatusId { get; set; }

        public long TicketDepartmentId { get; set; }

        public string TicketUserName { get; set; }

        public long UserId { get; set; }

        public string EmpCompanyId { get; set; }
    }
}
