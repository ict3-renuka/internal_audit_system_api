using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("DraftObservation")]
    public class DraftObservation
    {
        [Key]
        public int observation_id { get; set; }

        public string area { get; set; } = string.Empty;

        public string subject { get; set; } = string.Empty;

        public string details { get; set; } = string.Empty;

        public string risk_and_root_cause { get; set; } = string.Empty;

        public string recommendation { get; set; } = string.Empty;

        public DateTime? creation_date { get; set; }
    }
}