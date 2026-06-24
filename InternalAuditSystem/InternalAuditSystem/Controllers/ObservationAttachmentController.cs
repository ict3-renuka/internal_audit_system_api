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
        private readonly ILogger<ObservationAttachmentController> _logger;

        public ObservationAttachmentController(ApplicationDbContext context, ILogger<ObservationAttachmentController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdf([FromForm] int observationId, IFormFile file)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file");
                return StatusCode(500, new { message = "Failed to upload file." });
            }
        }

        [HttpGet("download/by-observation/{observationId}")]
        public async Task<IActionResult> DownloadByObservation(int observationId)
        {
            try
            {
                var file = await _context.ObservationAttachments
                        .Where(x => x.observation_id == observationId)
                        .OrderByDescending(x => x.attachment_id)
                        .FirstOrDefaultAsync();

                if (file == null)
                    return NotFound();

                var safeFileName = file.file_name.Replace("\"", "");

                Response.Headers["Content-Disposition"] = $"inline; filename=\"{safeFileName}\"";

                return File(file.file_data, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download file");
                return StatusCode(500, new { message = "Failed to download file." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var file = await _context.ObservationAttachments.FindAsync(id);
                if (file == null) return NotFound();

                _context.ObservationAttachments.Remove(file);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete file");
                return StatusCode(500, new { message = "Failed to delete file." });
            }
        }
    }
}
