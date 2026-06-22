using InternalAuditSystem.Data;
using InternalAuditSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InternalAuditSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuditRequestController> _logger;

        public CompanyController(ApplicationDbContext context, ILogger<AuditRequestController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company company)
        {
            try
            {
                _context.Companies.Add(company);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Company created successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create company");
                return StatusCode(500, new { message = "Failed to create company." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                var companies = await _context.Companies.ToListAsync();

                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch companies");
                return StatusCode(500, new { message = "Failed to fetch companies." });
            }
        }
    }
}