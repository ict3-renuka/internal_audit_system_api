using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("AuditRequest")]
    public class AuditRequest
    {
        [Key]
        public int request_id { get; set; }

        public DateTime meeting_date { get; set; }

        public string description { get; set; } = string.Empty;

        public DateTime? preliminary_start_date { get; set; }

        public string audit_firm { get; set; } = string.Empty;

        public string audit_firm_person_name { get; set; } = string.Empty;

        public string audit_department { get; set; } = string.Empty;

        public DateTime? info_request_date { get; set; }

        public DateTime? info_submit_date { get; set; }

        public DateTime? field_work_start_date { get; set; }

        public DateTime? field_work_end_date { get; set; }

        public DateTime? exit_meeting_date { get; set; }

        public DateTime? management_discussion_date { get; set; }

        public DateTime? report_issued_date { get; set; }

        public DateTime? shared_to_board_date { get; set; }

        public DateTime? audit_committee_table_date { get; set; }

        public DateTime creation_date { get; set; }

        public DateTime last_modified_date { get; set; }
    }
}