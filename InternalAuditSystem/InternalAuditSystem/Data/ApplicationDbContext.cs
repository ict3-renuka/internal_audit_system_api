using InternalAuditSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Center> Centers { get; set; }

        public DbSet<Department> Departments{ get; set; }

        public DbSet<InternalDepartment> InternalDepartments { get; set; }

        public DbSet<AuditRequest> AuditRequests { get; set; }

        public DbSet<DraftObservation> DraftObservations { get; set; }

        public DbSet<ObservationDetails> ObservationDetails { get; set; }
    }
}
