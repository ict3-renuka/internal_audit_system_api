using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CenterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CenterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCenter([FromBody] Center center)
        {
            _context.Centers.Add(center);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Center created successfully"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCenters()
        {
            var centers = await _context.Centers.ToListAsync();

            return Ok(centers);
        }
    }
}