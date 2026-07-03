namespace InternalAuditSystem.Models.DTO
{
    public class AuditRequestReportDto
    {
        public int RequestId { get; set; }
        public DateTime? MeetingDate { get; set; }
        public string? AuditName { get; set; }
        public DateTime? PreliminaryStartDate { get; set; }
        public string? AuditFirm { get; set; }
        public string? AuditManager { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime? InfoRequestDate { get; set; }
        public DateTime? InfoSubmitDate { get; set; }
        public DateTime? FieldWorkStartDate { get; set; }
        public DateTime? FieldWorkEndDate { get; set; }
        public DateTime? ExitMeetingDate { get; set; }
        public DateTime? ManagementDiscussionDate { get; set; }
        public DateTime? ReportIssuedDate { get; set; }
        public DateTime? SharedToBoardDate { get; set; }
        public DateTime? AuditCommitteeTableDate { get; set; }
        public string? ReviewReference { get; set; }
        public string? Sector { get; set; }
        public string? CompanyName { get; set; }
        public string? Status { get; set; }
        public int? DepartmentId { get; set; }
        public int? CompanyId { get; set; }
        public int? DraftObservationId { get; set; }
        public int? ObservationDetailId { get; set; }
    }
}
