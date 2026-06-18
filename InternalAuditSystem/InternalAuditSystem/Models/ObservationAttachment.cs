using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Models
{
    [Table("ObservationAttachment")]
    public class ObservationAttachment
    {
        [Key]
        public int attachment_id { get; set; }

        public int observation_id { get; set; }

        public string file_name { get; set; } = string.Empty;

        public string file_type { get; set; } = string.Empty;

        public byte[] file_data { get; set; } = Array.Empty<byte>();

        public DateTime uploaded_date { get; set; } = DateTime.Now;
    }
}
