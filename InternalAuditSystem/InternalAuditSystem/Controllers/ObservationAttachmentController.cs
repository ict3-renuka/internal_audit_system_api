using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationAttachmentController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ObservationAttachmentController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdf([FromForm] int observationId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var attachment = new ObservationAttachment
            {
                observation_id = observationId,
                file_name = file.FileName,
                file_type = file.ContentType,
                file_data = memoryStream.ToArray()
            };

            _context.ObservationAttachments.Add(attachment);
            await _context.SaveChangesAsync();

            return Ok(new { attachment.attachment_id });
        }

        [HttpGet("download/by-observation/{observationId}")]
        public async Task<IActionResult> DownloadByObservation(int observationId)
        {
            var file = await _context.ObservationAttachments
                    .Where(x => x.observation_id == observationId)
                    .OrderByDescending(x => x.attachment_id)
                    .FirstOrDefaultAsync();

            if (file == null)
                return NotFound();

            Response.Headers["Content-Disposition"] =$"inline; filename=\"{file.file_name}.pdf\"";

            return File(file.file_data, "application/pdf");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var file = await _context.ObservationAttachments.FindAsync(id);
            if (file == null) return NotFound();

            _context.ObservationAttachments.Remove(file);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
