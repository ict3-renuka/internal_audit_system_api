using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int user_id { get; set; }

        public int internal_department_id { get; set; }

        public string name { get; set; }

        public string designation { get; set; }

        public string user_name { get; set; }

        public string password { get; set; }

        public bool IsActive { get; set; }

        public string email { get; set; }

        public DateTime creation_date { get; set; }

        public DateTime last_modified_date { get; set; }
    }
}