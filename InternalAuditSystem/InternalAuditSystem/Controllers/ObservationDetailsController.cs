using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ObservationDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateObservationDetails(ObservationDetails request)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_InsertObservationDetails " +
                "@observation_id, @department_id, @internal_department_id, " +
                "@responsible_user, @management_response, @corrective_action_plan, " +
                "@action_time_line, @status, @remark, @remarked_date",

                new SqlParameter("@observation_id", request.observation_id),
                new SqlParameter("@department_id", request.department_id),
                new SqlParameter("@internal_department_id", request.internal_department_id),
                new SqlParameter("@responsible_user", (object?)request.responsible_user ?? DBNull.Value),
                new SqlParameter("@management_response", (object?)request.management_response ?? DBNull.Value),
                new SqlParameter("@corrective_action_plan", (object?)request.corrective_action_plan ?? DBNull.Value),
                new SqlParameter("@action_time_line", (object?)request.action_time_line ?? DBNull.Value),
                new SqlParameter("@status", (object?)request.status ?? DBNull.Value),
                new SqlParameter("@remark", (object?)request.remark ?? DBNull.Value),
                new SqlParameter("@remarked_date", (object?)request.remarked_date ?? DBNull.Value)
            );

            return Ok(new { message = "Observation Details created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllObservationDetails()
        {
            var data = await _context.ObservationDetails.ToListAsync();
            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObservationDetails(int id, ObservationDetails request)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_UpdateObservationDetails " +
                "@observation_details_id, @management_response, @corrective_action_plan, " +
                "@action_time_line, @status, @remark, @remarked_date",

                new SqlParameter("@observation_details_id", id),
                new SqlParameter("@management_response", (object?)request.management_response ?? DBNull.Value),
                new SqlParameter("@corrective_action_plan", (object?)request.corrective_action_plan ?? DBNull.Value),
                new SqlParameter("@action_time_line", (object?)request.action_time_line ?? DBNull.Value),
                new SqlParameter("@status", (object?)request.status ?? DBNull.Value),
                new SqlParameter("@remark", (object?)request.remark ?? DBNull.Value),
                new SqlParameter("@remarked_date", (object?)request.remarked_date ?? DBNull.Value)
            );

            return Ok(new { message = "Observation Details updated successfully" });
        }
    }
}