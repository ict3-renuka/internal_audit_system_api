namespace InternalAuditSystem.Models.DTO
{
    public class CombinedObservation
    {
        public int ObservationId { get; set; }
        public string ReviewReference { get; set; }
        public string Area { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public string RiskAndRootCause { get; set; }
        public string Recommendation { get; set; }
        public DateTime ObservationCreationDate { get; set; }
        public int? ObservationDetailsId { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }          
        public string? InternalDepartmentName { get; set; }
        public int? InternalDepartmentId { get; set; }
        public string? ResponsibleUser { get; set; }
        public string? ManagementResponse { get; set; }
        public string? CorrectiveActionPlan { get; set; }
        public DateTime? ActionTimeLine { get; set; }
        public string? Status { get; set; }
        public string? Remark { get; set; }
        public DateTime? RemarkedDate { get; set; }
        public bool IsActive { get; set; }
        public bool HasPdf { get; set; }
        public int? AttachmentId { get; set; }
        public string? FileName { get; set; }
    }
}
