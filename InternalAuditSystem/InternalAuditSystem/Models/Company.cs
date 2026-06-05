using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public int company_id { get; set; }

        public int sector_id { get; set; }

        public string sector_name { get; set; } = string.Empty;

        public string company_name { get; set; } = string.Empty;
    }
}
