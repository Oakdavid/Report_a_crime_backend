﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly ICategoryService _categoryService;

        public ReportController(IReportService reportService, ICategoryService categoryService)
        {
            _reportService = reportService;
            _categoryService = categoryService;
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
                    Data = report
                });
            }
            return BadRequest(new
            {
                Status = false,
                StatusCode = 400,
                Message = report.Message
            });
        }

        [HttpGet("GetAllReports")]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            if (reports.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "success",
                    data = reports.Select(p => new
                    {
                        p.ReportId,
                        p.NameOfTheOffender,
                        p.HeightOfTheOffender,
                        p.DateOccurred,
                        p.CreatedAt,
                        p.UploadEvidenceUrl,
                        p.Location,
                        p.DidItHappenInYourPresence,
                        p.CategoryName,
                        p.ReportDescription,
                        UserId = p.User?.UserId,
                        UserName = p.User?.FirstName
                    })
                });
            }
            return NotFound(new { Message = "No reports found." });
        }

        [HttpGet("GetAllReportsByUserAsync")]
        [Authorize]
        public async Task<IActionResult> GetAllReportsByUserAsync()
        {
            var reports = await _reportService.GetAllReportsByUserAsync();
            if (reports.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "success",
                    data = reports.Select(p => new
                    {
                        p.ReportId,
                        p.NameOfTheOffender,
                        p.HeightOfTheOffender,
                        p.DateOccurred,
                        p.UploadEvidenceUrl,
                        p.Location,
                        p.DidItHappenInYourPresence,
                        p.CategoryName,
                        p.ReportDescription
                    })
                });
            }
            return NotFound(new { Message = "No reports found." });
        }
    }
}
