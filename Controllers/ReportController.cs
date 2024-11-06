using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Services.Interface;

namespace Report_A_Crime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("ReportCrime")]
        public async Task<IActionResult> ReportCrime([FromForm] ReportRequestModel reportModel)
        {
            var report = await _reportService.CreateReportAsync(reportModel);

            if (report == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = report.Message,
                    StatusCode = report.Status // 400
                });
            }

            if (report.Status)
            {
                return Ok(new
                {
                    //Status = true,
                    //StatusCode = 200,
                    //Message = report.Message,
                    Data = report
                });
            }
            return BadRequest(new
            {
                Status = false,
                StatusCode = 400,
                Message = report.Message // failed to create report
            });
        }
    }
}
