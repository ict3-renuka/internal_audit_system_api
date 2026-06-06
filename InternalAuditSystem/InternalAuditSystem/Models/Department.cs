using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        public int department_id { get; set; }

        public int company_id { get; set; }

        public string department_name { get; set; } = string.Empty;
    }
}
