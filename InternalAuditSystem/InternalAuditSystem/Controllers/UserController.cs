using InternalAuditSystem.Data;
using InternalAuditSystem.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.user_name == request.user_name);

            if (user == null)
                return BadRequest(new { message = "Invalid username or password" });

            if (!user.IsActive)
                return BadRequest(new { message = "User is deactivated" });

            if (user.password != request.password)
                return BadRequest(new { message = "Invalid username or password" });

            var response = new LoginResponse
            {
                user_id = user.user_id,
                name = user.name,
                user_name = user.user_name,
                designation = user.designation,
                email = user.email,
                internal_department_id = user.internal_department_id,
                isActive = user.IsActive
            };

            return Ok(response);
        }

        [HttpGet("by-internal-department/{internalDepartmentId}")]
        public async Task<IActionResult> GetUsersByInternalDepartment(int internalDepartmentId)
        {
            var users = await _context.Users
                .Where(x => x.internal_department_id == internalDepartmentId && x.IsActive)
                .Select(x => new
                {
                    x.user_id,
                    x.name,
                    x.user_name,
                    x.designation,
                    x.internal_department_id
                })
                .ToListAsync();

            return Ok(users);
        }
    }
}
