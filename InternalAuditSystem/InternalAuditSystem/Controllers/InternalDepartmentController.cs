using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalDepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InternalDepartmentController> _logger;

        public InternalDepartmentController(ApplicationDbContext context, ILogger<InternalDepartmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInternalDepartment(InternalDepartment internalDepartment)
        {
            try
            {
                _context.InternalDepartments.Add(internalDepartment);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Internal Department created successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create internal department");
                return StatusCode(500, new { message = "Failed to create internal department." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInternalDepartments()
        {
            try
            {
                var internalDepartments = await _context.InternalDepartments
                    .OrderBy(d => d.internal_department_name)
                    .ToListAsync();

                return Ok(internalDepartments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch internal departments");
                return StatusCode(500, new { message = "Failed to fetch internal departments." });
            }
        }
    }
}
