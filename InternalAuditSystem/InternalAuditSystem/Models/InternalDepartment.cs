using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("InternalDepartment")]
    public class InternalDepartment
    {
        [Key]
        public int internal_department_id { get; set; }

        public int department_id { get; set; }

        public string internal_department_name { get; set; } = string.Empty;
    }
}
