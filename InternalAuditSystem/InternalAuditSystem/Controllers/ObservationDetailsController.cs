using Azure;
using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using InternalAuditSystem.Models.DTO;
using InternalAuditSystem.Services;
using InternalAuditSystem.Services.Report;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using System.Data;
using System.Text.Json;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<ObservationDetailsController> _logger;

        public ObservationDetailsController(ApplicationDbContext context, IEmailService emailService, ILogger<ObservationDetailsController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateObservationDetails(ObservationDetails request)
        {
            try
            {
                var idParam = new SqlParameter("@new_id", SqlDbType.Int) { Direction = ParameterDirection.Output };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_InsertObservationDetails " +
                    "@observation_id, @department_id, @internal_department_id, " +
                    "@responsible_user, @management_response, @corrective_action_plan, " +
                    "@action_time_line, @status, @remark, @remarked_date, @amendment_management_response, @new_id OUTPUT",
                    new SqlParameter("@observation_id", request.observation_id),
                    new SqlParameter("@department_id", request.department_id),
                    new SqlParameter("@internal_department_id", request.internal_department_id),
                    new SqlParameter("@responsible_user", (object?)request.responsible_user ?? DBNull.Value),
                    new SqlParameter("@management_response", (object?)request.management_response ?? DBNull.Value),
                    new SqlParameter("@corrective_action_plan", (object?)request.corrective_action_plan ?? DBNull.Value),
                    new SqlParameter("@action_time_line", (object?)request.action_time_line ?? DBNull.Value),
                    new SqlParameter("@status", (object?)request.status ?? DBNull.Value),
                    new SqlParameter("@remark", (object?)request.remark ?? DBNull.Value),
                    new SqlParameter("@remarked_date", (object?)request.remarked_date ?? DBNull.Value),
                    new SqlParameter("@amendment_management_response", (object?)request.amendment_management_response ?? DBNull.Value),
                    idParam
                );

                int newId = idParam.Value is int value ? value : 0;

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
                    observation_details_id = newId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create observation detail");
                return StatusCode(500, new { error = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllObservationDetails()
        {
            try
            {
                var data = await _context.ObservationDetails.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch observation details");
                return StatusCode(500, new { message = "Failed to fetch observation details." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObservationDetails(int id, [FromBody] Dictionary<string, object?> fields)
        {
            try
            {
                fields.TryGetValue("management_response", out var mgmtResponse);
                fields.TryGetValue("corrective_action_plan", out var cap);
                fields.TryGetValue("action_time_line", out var atl);
                fields.TryGetValue("status", out var status);
                fields.TryGetValue("remark", out var remark);
                fields.TryGetValue("remarked_date", out var rd);
                fields.TryGetValue("is_active", out var isActive);
                fields.TryGetValue("amendment_management_response", out var amr);

                object isActiveValue = DBNull.Value;

                if (isActive is JsonElement je)
                {
                    if (je.ValueKind == JsonValueKind.True || je.ValueKind == JsonValueKind.False)
                        isActiveValue = je.GetBoolean();
                }
                else if (isActive is bool b)
                {
                    isActiveValue = b;
                }

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_UpdateObservationDetails " +
                    "@observation_details_id, @management_response, @corrective_action_plan, " +
                    "@action_time_line, @status, @remark, @remarked_date, @is_active, @amendment_management_response",
                    new SqlParameter("@observation_details_id", id),
                    new SqlParameter("@management_response", (object?)mgmtResponse?.ToString() ?? DBNull.Value),
                    new SqlParameter("@corrective_action_plan", (object?)cap?.ToString() ?? DBNull.Value),
                    new SqlParameter("@action_time_line", (object?)atl?.ToString() ?? DBNull.Value),
                    new SqlParameter("@status", (object?)status?.ToString() ?? DBNull.Value),
                    new SqlParameter("@remark", (object?)remark?.ToString() ?? DBNull.Value),
                    new SqlParameter("@remarked_date", (object?)rd?.ToString() ?? DBNull.Value),
                    new SqlParameter("@is_active", isActiveValue),
                    new SqlParameter("@amendment_management_response", (object?)amr?.ToString() ?? DBNull.Value)
                    );

                return Ok(new { message = "Observation Details updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update observation details");
                return StatusCode(500, new { message = "Failed to update observation details." });
            }
        }

        [HttpGet("combined")]
        public async Task<IActionResult> GetCombinedObservations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool includeInactive = false)
        {
            try
            {
                        var query =
                            from od in _context.ObservationDetails

                            join o in _context.DraftObservations
                                on od.observation_id equals o.observation_id

                            join d in _context.Departments
                                on od.department_id equals d.department_id into depts
                            from d in depts.DefaultIfEmpty()

                            join id in _context.InternalDepartments
                                on od.internal_department_id equals id.internal_department_id into intDepts
                            from id in intDepts.DefaultIfEmpty()

                            where includeInactive || od.is_active

                            orderby o.creation_date descending
                            select new CombinedObservation
                            {
                                AuditRequestId = o.audit_request_id,
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
                                AmendmentManagementResponse = od != null ? od.amendment_management_response : null,
                                IsActive = od != null ? od.is_active : true,
                                HasPdf = _context.ObservationAttachments
                                        .Any(a => a.observation_id == o.observation_id),

                                AttachmentId = _context.ObservationAttachments
                                                .Where(a => a.observation_id == o.observation_id)
                                                .OrderByDescending(a => a.attachment_id)
                                                .Select(a => (int?)a.attachment_id)
                                                .FirstOrDefault(),

                                FileName = _context.ObservationAttachments
                                            .Where(a => a.observation_id == o.observation_id)
                                            .OrderByDescending(a => a.attachment_id)
                                            .Select(a => a.file_name)
                                            .FirstOrDefault()
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get combined observations");
                return StatusCode(500, new { message = "Failed to get combined observations." });
            }
        }

        [HttpGet("byObservation/{observationId}")]
        public async Task<IActionResult> GetInternalDeptIdsByObservationId(int observationId)
        {
            try
            {
                var internalDepartmentIds = await _context.ObservationDetails
                    .Where(od => od.observation_id == observationId && od.is_active)
                    .Select(od => od.internal_department_id)
                    .ToListAsync();

                return Ok(internalDepartmentIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch internal department");
                return StatusCode(500, new { message = "Failed to fetch internal department." });
            }
        }

        [HttpGet("observation-report")]
        public async Task<IActionResult> GetReport(
            [FromQuery] int? departmentId,
            [FromQuery] int? internalDepartmentId,
            [FromQuery] string? status,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            try
            {
                var query = from o in _context.DraftObservations
                            join od in _context.ObservationDetails
                                on o.observation_id equals od.observation_id into odGroup
                            from od in odGroup.DefaultIfEmpty()

                            join ar in _context.AuditRequests
                        on o.audit_request_id equals ar.request_id

                            join d in _context.Departments
                        on od.department_id equals d.department_id into dGroup
                    from d in dGroup.DefaultIfEmpty()

                    join id in _context.InternalDepartments
                        on od.internal_department_id equals id.internal_department_id into idGroup
                    from id in idGroup.DefaultIfEmpty()
                           // where od.is_active == true
                    select new CombinedObservation
                    {
                                ObservationId = o.observation_id,
                                DepartmentId = d.department_id,
                                InternalDepartmentId = id.internal_department_id,
                        Area = o.area,
                        Subject = o.subject,
                        AuditName = ar.audit_name,
                        RiskAndRootCause = o.risk_and_root_cause,
                        Recommendation = o.recommendation,
                        DepartmentName = d.department_name,
                        InternalDepartmentName = id.internal_department_name,
                        ManagementResponse = od.management_response,
                        CorrectiveActionPlan = od.corrective_action_plan,
                        Status = od.status,
                        Remark = od.remark,
                        AmendmentManagementResponse = od.amendment_management_response,
                        ObservationCreationDate = o.creation_date
                    };

                if (departmentId.HasValue)
                    query = query.Where(x => x.DepartmentId == departmentId.Value);

                if (internalDepartmentId.HasValue)
                    query = query.Where(x => x.InternalDepartmentId == internalDepartmentId.Value);

                if (!string.IsNullOrWhiteSpace(status))
                    query = query.Where(x => x.Status == status);

                if (fromDate.HasValue)
                    query = query.Where(x => x.ObservationCreationDate >= fromDate);

                if (toDate.HasValue)
                    query = query.Where(x => x.ObservationCreationDate <= toDate);

                var data = await query
                            .OrderByDescending(o => o.ObservationCreationDate)
                            .ToListAsync();

                var document = new ObservationReportDocument(data);
                var pdf = document.GeneratePdf();

                //return File(pdf, "application/pdf", "ObservationReport.pdf");
                Response.Headers["Content-Disposition"] = "inline; filename=ObservationReport.pdf";
                return File(pdf, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch report");
                return StatusCode(500, new { message = "Failed to fetch report." });
            }
        }

    }
}