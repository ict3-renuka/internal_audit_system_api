using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("ObservationDetails")]
    public class ObservationDetails
    {
        [Key]
        public int observation_details_id { get; set; }

        public int observation_id { get; set; }

        public int department_id { get; set; }

        public int internal_department_id { get; set; }

        public string? responsible_user { get; set; }

        public string? management_response { get; set; }

        public string? corrective_action_plan { get; set; }

        public int? action_time_line { get; set; }

        public string? status { get; set; }

        public string? remark { get; set; }

        public DateTime? remarked_date { get; set; }

        public DateTime creation_date { get; set; }

        public DateTime last_modified_date { get; set; }
    }
}