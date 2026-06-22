using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(ApplicationDbContext context, ILogger<DepartmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            try
            {
                _context.Departments.Add(department);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Department created successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create department");
                return StatusCode(500, new { message = "Failed to create department." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var departments = await _context.Departments
                    .OrderBy(d => d.department_name)
                    .ToListAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch departments");
                return StatusCode(500, new { message = "Failed to fetch departments." });
            }
        }
    }
}
