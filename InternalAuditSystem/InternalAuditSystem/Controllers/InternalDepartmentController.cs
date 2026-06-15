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

        public InternalDepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInternalDepartment(InternalDepartment internalDepartment)
        {
            _context.InternalDepartments.Add(internalDepartment);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Internal Department created successfully"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInternalDepartments()
        {
            var internalDepartments = await _context.InternalDepartments
                .OrderBy(d => d.internal_department_name)
                .ToListAsync();

            return Ok(internalDepartments);
        }
    }
}
