using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI_Model
{
    public class PROC_Ticket_Custom_Search_Model
    {
        public long TicketID { get; set; }
        public string TicketNo { get; set; }
        public string TicketSubject { get; set; }
        public string TicketDescription { get; set; }
        public string TicketAttachmentmentPath { get; set; }
        public int? TicketDepartmentID { get; set; }
        public decimal? TicketStatusID { get; set; }
        public DateTime? TicketDateTime { get; set; }
        public decimal? TicketPriority { get; set; }
        public decimal? TicketTransfer { get; set; }
        public string TicketUserName { get; set; }
        public int? TicketUserID { get; set; }
        public int? TicketTypeId { get; set; }
        public bool? TicketBroadCasted { get; set; }
        public int? TicketSubjectID { get; set; }
        public DateTime? TicketDeadline { get; set; }
        public DateTime? UpdatedTicketDeadline { get; set; }
        public System.DateTime ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public int? WorkDone { get; set; }
        public int? DeploymentStatusId { get; set; }
        public long? ParentTicketId { get; set; }
        public int? TicketTypeInfoID { get; set; }
        public string TicketStatusDescription { get; set; }
        public string TicketPriorityDescription { get; set; }
        public string TicketDepartmentName { get; set; }
        public decimal? TStatusId { get; set; }
        public string TicketTypeName { get; set; }
        public string TicketTypeShortName { get; set; }
        public string TicketSubjectName { get; set; }
        public int MarkAsRead { get; set; }
        public int ImportantStatus { get; set; }
        public int? LocationID { get; set; }
        public bool? TicketAcknowledged { get; set; }
        public string LastComment { get; set; }
        public bool? TicketAckRequired { get; set; }
        public string TicketAssignedUser { get; set; }
        public string TicketSubType { get; set; }

        public string AssignedUser { get; set; }
        public string CommentUserId { get; set; }
        public string CommentUserName { get; set; }

        public string BillMonth { get; set; }
        public string InvoiceNo { get; set; }

        public bool? HasSubTaskTicket { get; set; }
    }
}
