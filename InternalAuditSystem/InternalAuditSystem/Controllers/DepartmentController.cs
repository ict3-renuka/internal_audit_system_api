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

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Department created successfully"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _context.Departments
                .OrderBy(d => d.department_name)
                .ToListAsync();
            return Ok(departments);
        }
    }
}
