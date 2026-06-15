namespace InternalAuditSystem.Models.DTO
{
    public class LoginResponse
    {
        public int user_id { get; set; }
        public string name { get; set; }
        public string user_name { get; set; }
        public string designation { get; set; }
        public string email { get; set; }
        public int internal_department_id { get; set; }
        public bool isActive { get; set; }
    }
}
