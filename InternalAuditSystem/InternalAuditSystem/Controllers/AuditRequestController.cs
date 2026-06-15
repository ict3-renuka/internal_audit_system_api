using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuditRequest([FromBody] AuditRequest request)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_InsertAuditRequest " +
                "@meeting_date, @description, @preliminary_start_date, " +
                "@audit_firm, @audit_firm_person_name, @audit_department_id, " +
                "@info_request_date, @info_submit_date, @field_work_start_date, " +
                "@field_work_end_date, @exit_meeting_date, @management_discussion_date, " +
                "@report_issued_date, @shared_to_board_date, @audit_committee_table_date",

                new SqlParameter("@meeting_date", request.meeting_date),
                new SqlParameter("@description", request.description),
                new SqlParameter("@preliminary_start_date", (object?)request.preliminary_start_date ?? DBNull.Value),
                new SqlParameter("@audit_firm", request.audit_firm),
                new SqlParameter("@audit_firm_person_name", request.audit_firm_person_name),
                new SqlParameter("@audit_department_id", request.audit_department_id),
                new SqlParameter("@info_request_date", (object?)request.info_request_date ?? DBNull.Value),
                new SqlParameter("@info_submit_date", (object?)request.info_submit_date ?? DBNull.Value),
                new SqlParameter("@field_work_start_date", (object?)request.field_work_start_date ?? DBNull.Value),
                new SqlParameter("@field_work_end_date", (object?)request.field_work_end_date ?? DBNull.Value),
                new SqlParameter("@exit_meeting_date", (object?)request.exit_meeting_date ?? DBNull.Value),
                new SqlParameter("@management_discussion_date", (object?)request.management_discussion_date ?? DBNull.Value),
                new SqlParameter("@report_issued_date", (object?)request.report_issued_date ?? DBNull.Value),
                new SqlParameter("@shared_to_board_date", (object?)request.shared_to_board_date ?? DBNull.Value),
                new SqlParameter("@audit_committee_table_date", (object?)request.audit_committee_table_date ?? DBNull.Value)
            );

            return Ok(new { message = "Audit Request created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuditRequests([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var totalCount = await _context.AuditRequests.CountAsync();

            var auditRequests = await _context.AuditRequests
                .OrderByDescending(a => a.creation_date)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuditRequest(int id, AuditRequest request)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC sp_UpdateAuditRequest
            @request_id,
            @preliminary_start_date,
            @info_request_date,
            @info_submit_date,
            @field_work_start_date,
            @field_work_end_date,
            @exit_meeting_date,
            @management_discussion_date,
            @report_issued_date,
            @shared_to_board_date,
            @audit_committee_table_date",

                new SqlParameter("@request_id", id),
                new SqlParameter("@preliminary_start_date", (object?)request.preliminary_start_date ?? DBNull.Value),
                new SqlParameter("@info_request_date", (object?)request.info_request_date ?? DBNull.Value),
                new SqlParameter("@info_submit_date", (object?)request.info_submit_date ?? DBNull.Value),
                new SqlParameter("@field_work_start_date", (object?)request.field_work_start_date ?? DBNull.Value),
                new SqlParameter("@field_work_end_date", (object?)request.field_work_end_date ?? DBNull.Value),
                new SqlParameter("@exit_meeting_date", (object?)request.exit_meeting_date ?? DBNull.Value),
                new SqlParameter("@management_discussion_date", (object?)request.management_discussion_date ?? DBNull.Value),
                new SqlParameter("@report_issued_date", (object?)request.report_issued_date ?? DBNull.Value),
                new SqlParameter("@shared_to_board_date", (object?)request.shared_to_board_date ?? DBNull.Value),
                new SqlParameter("@audit_committee_table_date", (object?)request.audit_committee_table_date ?? DBNull.Value)
            );

            return Ok(new { message = "Audit Request updated successfully" });
        }
    }
}