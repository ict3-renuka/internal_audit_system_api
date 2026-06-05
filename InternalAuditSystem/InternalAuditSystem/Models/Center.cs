using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("Center")]
    public class Center
    {
        [Key]
        public int center_id { get; set; }

        public int company_id { get; set; }

        public string center_name { get; set; } = string.Empty;
    }
}