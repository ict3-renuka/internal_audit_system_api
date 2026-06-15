using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using InternalAuditSystem.Models.DTO;
using InternalAuditSystem.Services;
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
        private readonly IEmailService _emailService;

        public ObservationDetailsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateObservationDetails(ObservationDetails request)
        {
            try
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

                var observation = await _context.DraftObservations
                    .FirstOrDefaultAsync(o => o.observation_id == request.observation_id);

                var users = await _context.Users
                    .Where(u => u.internal_department_id == request.internal_department_id
                             && u.IsActive
                             && !string.IsNullOrEmpty(u.email))
                    .ToListAsync();

                foreach (var user in users)
                {
                    try
                    {
                        await _emailService.SendEmailAsync(
                            toEmail: user.email,
                            toName: user.name,
                            subject: "New Internal Audit Observation Details Submitted",
                            body: $@"
                        <p>Dear {user.name},</p>
                        <p>A new observation has been submitted for your department.</p>
                        <table border='1' cellpadding='8' cellspacing='0' style='border-collapse: collapse;'>
                            <tr><td><b>Observation ID</b></td><td>{request.observation_id}</td></tr>
                            <tr><td><b>Area</b></td><td>{observation?.area ?? "—"}</td></tr>
                            <tr><td><b>Subject</b></td><td>{observation?.subject ?? "—"}</td></tr>
                        </table>
                        <p>Please log in to the Internal Audit System and fill your details.</p>
                        <p>Regards,<br/>Internal Audit Team</p>
                    "
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Email failed for {user.email}: {ex.Message}");
                        Console.WriteLine($"Inner: {ex.InnerException?.Message}");
                    }
                }

                return Ok(new
                {
                    message = "Observation Details created successfully",
                    observation_details_id = request.observation_details_id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllObservationDetails()
        {
            var data = await _context.ObservationDetails.ToListAsync();
            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObservationDetails(int id, [FromBody] Dictionary<string, object?> fields)
        {
            fields.TryGetValue("management_response", out var mgmtResponse);
            fields.TryGetValue("corrective_action_plan", out var cap);
            fields.TryGetValue("action_time_line", out var atl);
            fields.TryGetValue("status", out var status);
            fields.TryGetValue("remark", out var remark);
            fields.TryGetValue("remarked_date", out var rd);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_UpdateObservationDetails " +
                "@observation_details_id, @management_response, @corrective_action_plan, " +
                "@action_time_line, @status, @remark, @remarked_date",
                new SqlParameter("@observation_details_id", id),
                new SqlParameter("@management_response", (object?)mgmtResponse?.ToString() ?? DBNull.Value),
                new SqlParameter("@corrective_action_plan", (object?)cap?.ToString() ?? DBNull.Value),
                new SqlParameter("@action_time_line", (object?)atl?.ToString() ?? DBNull.Value),
                new SqlParameter("@status", (object?)status?.ToString() ?? DBNull.Value),
                new SqlParameter("@remark", (object?)remark?.ToString() ?? DBNull.Value),
                new SqlParameter("@remarked_date", (object?)rd?.ToString() ?? DBNull.Value)
            );

            return Ok(new { message = "Observation Details updated successfully" });
        }

        [HttpGet("combined")]
        public async Task<IActionResult> GetCombinedObservations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        {
            var query = from o in _context.DraftObservations

                        join od in _context.ObservationDetails
                            on o.observation_id equals od.observation_id into details
                        from od in details.DefaultIfEmpty()

                        join d in _context.Departments
                            on od.department_id equals d.department_id into depts
                        from d in depts.DefaultIfEmpty()

                        join id in _context.InternalDepartments
                            on od.internal_department_id equals id.internal_department_id into intDepts
                        from id in intDepts.DefaultIfEmpty()

                        orderby o.creation_date descending
                        select new CombinedObservation
                        {
                            ObservationId = o.observation_id,
                            Area = o.area,
                            Subject = o.subject,
                            Details = o.details,
                            RiskAndRootCause = o.risk_and_root_cause,
                            Recommendation = o.recommendation,
                            ObservationCreationDate = o.creation_date,
                            ObservationDetailsId = od != null ? od.observation_details_id : (int?)null,
                            DepartmentName = d != null ? d.department_name : null,
                            InternalDepartmentName = id != null ? id.internal_department_name : null,
                            InternalDepartmentId = od != null ? od.internal_department_id : (int?)null,
                            ResponsibleUser = od != null ? od.responsible_user : null,
                            ManagementResponse = od != null ? od.management_response : null,
                            CorrectiveActionPlan = od != null ? od.corrective_action_plan : null,
                            ActionTimeLine = od != null ? od.action_time_line : (DateTime?)null,
                            Status = od != null ? od.status : null,
                            Remark = od != null ? od.remark : null,
                            RemarkedDate = od != null ? od.remarked_date : (DateTime?)null,
                        };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                total_count = totalCount,
                page,
                page_size = pageSize,
                total_pages = (int)Math.Ceiling(totalCount / (double)pageSize),
                items
            });
        }

        [HttpGet("byObservation/{observationId}")]
        public async Task<IActionResult> GetInternalDeptIdsByObservationId(int observationId)
        {
            var internalDepartmentIds = await _context.ObservationDetails
                .Where(od => od.observation_id == observationId)
                .Select(od => od.internal_department_id)
                .ToListAsync();

            return Ok(internalDepartmentIds);
        }

    }
}