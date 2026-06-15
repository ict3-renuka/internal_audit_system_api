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

        public DraftObservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDraftObservation(DraftObservation observation)
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

        [HttpGet]
        public async Task<IActionResult> GetAllDraftObservations()
        {
            var observations = await _context.DraftObservations.ToListAsync();

            return Ok(observations);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDraftObservation(int id, DraftObservation observation)
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
    }
}