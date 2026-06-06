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
                message = "Draft Observation created successfully"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDraftObservations()
        {
            var observations = await _context.DraftObservations.ToListAsync();

            return Ok(observations);
        }
    }
}