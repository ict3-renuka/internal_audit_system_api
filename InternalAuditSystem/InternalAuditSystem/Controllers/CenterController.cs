using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CenterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CenterController> _logger;

        public CenterController(ApplicationDbContext context, ILogger<CenterController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCenter([FromBody] Center center)
        {
            try
            {
                _context.Centers.Add(center);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Center created successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create center");
                return StatusCode(500, new { message = "Failed to create center." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCenters()
        {
            try
            {
                var centers = await _context.Centers.ToListAsync();

                return Ok(centers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch centers");
                return StatusCode(500, new { message = "Failed to fetch centers." });
            }
        }
    }
}