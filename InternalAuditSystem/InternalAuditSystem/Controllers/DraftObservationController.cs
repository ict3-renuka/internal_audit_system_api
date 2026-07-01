using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftObservationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DraftObservationController> _logger;

        public DraftObservationController(ApplicationDbContext context, ILogger<DraftObservationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDraftObservation(DraftObservation observation)
        {
            try
            {
                observation.creation_date = DateTime.Now;
                _context.DraftObservations.Add(observation);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Draft Observation created successfully",
                    observation_id = observation.observation_id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create draft observation");
                return StatusCode(500, new { message = "Failed to create draft observation." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDraftObservations()
        {
            try
            {
                var observations = await _context.DraftObservations.ToListAsync();

                return Ok(observations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch draft observations");
                return StatusCode(500, new { message = "Failed to fetch draft observations." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDraftObservation(int id, DraftObservation observation)
        {
            try
            {
                var existing = await _context.DraftObservations.FindAsync(id);
                if (existing == null) return NotFound();

                existing.area = observation.area;
                existing.subject = observation.subject;
                existing.details = observation.details;
                existing.risk_and_root_cause = observation.risk_and_root_cause;
                existing.recommendation = observation.recommendation;

                await _context.SaveChangesAsync();
                return Ok(new { message = "Draft Observation updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update draft observation");
                return StatusCode(500, new { message = "Failed to update draft observation." });
            }
        }
    }
}