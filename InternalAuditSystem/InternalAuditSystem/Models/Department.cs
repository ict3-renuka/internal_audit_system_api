using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        public int internal_department_id { get; set; }

        public int company_id { get; set; }

        public string audit_department_name { get; set; } = string.Empty;

        public string internal_department_name { get; set; } = string.Empty;
    }
}
