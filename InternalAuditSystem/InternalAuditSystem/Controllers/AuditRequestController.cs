using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using InternalAuditSystem.Models.DTO;
using InternalAuditSystem.Services.Report;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuditRequestController> _logger;

        public AuditRequestController(ApplicationDbContext context, ILogger<AuditRequestController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuditRequest([FromBody] AuditRequest request)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_InsertAuditRequest " +
                    "@meeting_date, @audit_name, @preliminary_start_date, " +
                    "@audit_firm, @audit_manager, @audit_department_id, " +
                    "@info_request_date, @info_submit_date, @field_work_start_date, " +
                    "@field_work_end_date, @exit_meeting_date, @management_discussion_date, " +
                    "@report_issued_date, @shared_to_board_date, @audit_committee_table_date, " +
                    "@review_reference, @sector, @company_id, @management_response_received_date, " +
                    "@draft_report_received_date, @draft_report_circulate_date",

                    new SqlParameter("@meeting_date", request.meeting_date),
                    new SqlParameter("@audit_name", request.audit_name),
                    new SqlParameter("@preliminary_start_date", (object?)request.preliminary_start_date ?? DBNull.Value),
                    new SqlParameter("@audit_firm", request.audit_firm),
                    new SqlParameter("@audit_manager", request.audit_manager),
                    new SqlParameter("@audit_department_id", request.audit_department_id),
                    new SqlParameter("@info_request_date", (object?)request.info_request_date ?? DBNull.Value),
                    new SqlParameter("@info_submit_date", (object?)request.info_submit_date ?? DBNull.Value),
                    new SqlParameter("@field_work_start_date", (object?)request.field_work_start_date ?? DBNull.Value),
                    new SqlParameter("@field_work_end_date", (object?)request.field_work_end_date ?? DBNull.Value),
                    new SqlParameter("@exit_meeting_date", (object?)request.exit_meeting_date ?? DBNull.Value),
                    new SqlParameter("@management_discussion_date", (object?)request.management_discussion_date ?? DBNull.Value),
                    new SqlParameter("@report_issued_date", (object?)request.report_issued_date ?? DBNull.Value),
                    new SqlParameter("@shared_to_board_date", (object?)request.shared_to_board_date ?? DBNull.Value),
                    new SqlParameter("@audit_committee_table_date", (object?)request.audit_committee_table_date ?? DBNull.Value),
                    new SqlParameter("@review_reference", request.review_reference),
                    new SqlParameter("@sector", request.sector),
                    new SqlParameter("@company_id", request.company_id),
                    new SqlParameter("@management_response_received_date", (object?)request.management_response_received_date ?? DBNull.Value),
                    new SqlParameter("@draft_report_received_date", (object?)request.draft_report_received_date ?? DBNull.Value),
                    new SqlParameter("@draft_report_circulate_date", (object?)request.draft_report_circulate_date ?? DBNull.Value)
                );

                return Ok(new { message = "Audit Request created successfully" });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Failed to create audit request");
                return StatusCode(500, new { message = "Failed to create audit request." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuditRequests([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var totalCount = await _context.AuditRequests.CountAsync();

                var auditRequests = await _context.AuditRequests
                    .OrderByDescending(a => a.creation_date)
                    .ThenByDescending(a => a.request_id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    data = auditRequests,
                    totalCount = totalCount,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch audit requests");
                return StatusCode(500, new { message = "Failed to fetch audit requests." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuditRequest(int id, AuditRequest request)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC sp_UpdateAuditRequest
        @request_id,
        @meeting_date,
        @audit_name,
        @preliminary_start_date,
        @audit_firm,
        @audit_manager,
        @audit_department_id,
        @info_request_date,
        @info_submit_date,
        @field_work_start_date,
        @field_work_end_date,
        @exit_meeting_date,
        @management_discussion_date,
        @report_issued_date,
        @shared_to_board_date,
        @audit_committee_table_date,
        @review_reference,
        @sector,
        @company_id,
        @management_response_received_date,
        @draft_report_received_date,
        @draft_report_circulate_date",

                    new SqlParameter("@request_id", id),
                    new SqlParameter("@meeting_date", request.meeting_date),
                    new SqlParameter("@audit_name", request.audit_name),
                    new SqlParameter("@preliminary_start_date", (object?)request.preliminary_start_date ?? DBNull.Value),
                    new SqlParameter("@audit_firm", request.audit_firm),
                    new SqlParameter("@audit_manager", request.audit_manager),
                    new SqlParameter("@audit_department_id", request.audit_department_id),
                    new SqlParameter("@info_request_date", (object?)request.info_request_date ?? DBNull.Value),
                    new SqlParameter("@info_submit_date", (object?)request.info_submit_date ?? DBNull.Value),
                    new SqlParameter("@field_work_start_date", (object?)request.field_work_start_date ?? DBNull.Value),
                    new SqlParameter("@field_work_end_date", (object?)request.field_work_end_date ?? DBNull.Value),
                    new SqlParameter("@exit_meeting_date", (object?)request.exit_meeting_date ?? DBNull.Value),
                    new SqlParameter("@management_discussion_date", (object?)request.management_discussion_date ?? DBNull.Value),
                    new SqlParameter("@report_issued_date", (object?)request.report_issued_date ?? DBNull.Value),
                    new SqlParameter("@shared_to_board_date", (object?)request.shared_to_board_date ?? DBNull.Value),
                    new SqlParameter("@audit_committee_table_date", (object?)request.audit_committee_table_date ?? DBNull.Value),
                    new SqlParameter("@review_reference", request.review_reference),
                    new SqlParameter("@sector", request.sector),
                    new SqlParameter("@company_id", request.company_id),
                    new SqlParameter("@management_response_received_date", (object?)request.management_response_received_date ?? DBNull.Value),
                    new SqlParameter("@draft_report_received_date", (object?)request.draft_report_received_date ?? DBNull.Value),
                    new SqlParameter("@draft_report_circulate_date", (object?)request.draft_report_circulate_date ?? DBNull.Value)
                );

                return Ok(new { message = "Audit Request updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update audit request");
                return StatusCode(500, new { message = "Failed to update audit request." });
            }
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetAuditRequestById(int id)
        {
            try
            {
                var auditRequest = await _context.AuditRequests
                    .FirstOrDefaultAsync(x => x.request_id == id);

                if (auditRequest == null)
                {
                    return NotFound(new { message = "Audit request not found" });
                }
                return Ok(auditRequest);
            }
            catch (Exception ex)

            {
                _logger.LogError(ex, "Failed to fetch audit request by id");
                return StatusCode(500, new { message = "Failed to fetch audit request." });
            }
        }

        [HttpGet("audit-request-report")]
        public async Task<IActionResult> GetAuditRequestReport(
            string? sector,
            int? companyId,
            int? departmentId,
            string? status,
            DateTime? fromDate,
            DateTime? toDate)
                {
            try
            {
                var query =
                    from ar in _context.AuditRequests

                    join dofJoin in _context.DraftObservations
                        on ar.request_id equals dofJoin.audit_request_id into dofGroup
                    from dof in dofGroup.DefaultIfEmpty()

                    join odJoin in _context.ObservationDetails
                        on (dof != null ? (int?)dof.observation_id : null) equals odJoin.observation_id into odGroup
                    from od in odGroup.DefaultIfEmpty()

                    join d in _context.Departments
                        on ar.audit_department_id equals d.department_id into deptGroup
                    from d in deptGroup.DefaultIfEmpty()

                    join c in _context.Companies
                        on ar.company_id equals c.company_id into companyGroup
                    from c in companyGroup.DefaultIfEmpty()

                    select new AuditRequestReportDto
                    {
                        RequestId = ar.request_id,
                        MeetingDate = ar.meeting_date,
                        AuditName = ar.audit_name,
                        PreliminaryStartDate = ar.preliminary_start_date,
                        AuditFirm = ar.audit_firm,
                        AuditManager = ar.audit_manager,

                        DepartmentId = d != null ? (int?)d.department_id : null,
                        DepartmentName = d != null ? d.department_name : null,

                        InfoRequestDate = ar.info_request_date,
                        InfoSubmitDate = ar.info_submit_date,
                        FieldWorkStartDate = ar.field_work_start_date,
                        FieldWorkEndDate = ar.field_work_end_date,
                        ExitMeetingDate = ar.exit_meeting_date,
                        ManagementDiscussionDate = ar.management_discussion_date,
                        ReportIssuedDate = ar.report_issued_date,
                        SharedToBoardDate = ar.shared_to_board_date,
                        AuditCommitteeTableDate = ar.audit_committee_table_date,
                        ReviewReference = ar.review_reference,
                        Sector = ar.sector,

                        CompanyId = c != null ? (int?)c.company_id : null,
                        CompanyName = c != null ? c.company_name : null,

                        DraftObservationId = dof != null ? (int?)dof.observation_id : null,
                        ObservationDetailId = od != null ? (int?)od.observation_id : null,

                        Status = od != null ? od.status : null
                    };

                if (!string.IsNullOrWhiteSpace(sector))
                    query = query.Where(x => x.Sector == sector);
                if (departmentId.HasValue)
                    query = query.Where(x => x.DepartmentId == departmentId.Value);
                if (companyId.HasValue)
                    query = query.Where(x => x.CompanyId == companyId.Value);
                if (fromDate.HasValue)
                    query = query.Where(x => x.MeetingDate >= fromDate);
                if (toDate.HasValue)
                    query = query.Where(x => x.MeetingDate <= toDate);

                if (!string.IsNullOrWhiteSpace(status))
                    query = query.Where(x => x.Status == status);

                var data = await query
                    .OrderByDescending(x => x.MeetingDate)
                    .ToListAsync();

                var document = new AuditRequestReportDocument(data);
                var pdf = document.GeneratePdf();

                Response.Headers["Content-Disposition"] = "inline; filename=AuditRequestReport.pdf";
                return File(pdf, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate Audit Request report.");
                return StatusCode(500, new { message = "Failed to generate report." });
            }
        }
    }
}