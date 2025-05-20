using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting;
using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interface;
using Report_A_Crime.Models.Repositories.Interphase;
using Report_A_Crime.Models.Services.Interface;
using System;
using System.Drawing;
using System.Security.Claims;

namespace Report_A_Crime.Models.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGeolocationRepository _geolocationRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _environment;
        private readonly IGeolocationService _geolocationService;


        public ReportService(IReportRepository reportRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, ICategoryRepository categoryRepository, IHttpContextAccessor contextAccessor, IWebHostEnvironment environment, IGeolocationRepository geolocationRepository)
        {
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
           _categoryRepository = categoryRepository;
            _contextAccessor = contextAccessor;
            _environment = environment;
            _geolocationRepository = geolocationRepository;
        }

        public async Task<ReportDto> CreateReportAsync(ReportRequestModel reportModel)
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingReport = await _reportRepository.FindSimilarReportAsync(
                reportModel.CategoryName,
                reportModel.ReportDescription,
                DateTime.UtcNow.AddDays(-1)
            );

            if (existingReport != null)
            {
                return new ReportDto
                {
                    Message = "A similar report already exists",
                    Data = null,
                    Status = false,
                };
            }

            var category = await _categoryRepository.GetCategoryAsync(
                a => a.CategoryName == reportModel.CategoryName
            );

            var newReport = new Report
            {
                Category = category,
                DateOccurred = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                NameOfTheOffender = reportModel.NameOfTheOffender,
                Location = reportModel.Location,
                HeightOfTheOffender = reportModel.HeightOfTheOffender,
                DidItHappenInYourPresence = reportModel.DidItHappenInYourPresence,
                ReportDescription = reportModel.ReportDescription,
                ReportStatus = Enums.ReportStatus.UnderReview,
            };

            if (Guid.TryParse(userId, out var parsedUserId))
            {
                newReport.UserId = parsedUserId;
            }
            else
            {
                newReport.UserId = null;
            }

            if (reportModel.UploadEvidence != null && reportModel.UploadEvidence.Length > 0)
            {
                var uploadEvidenceUrl = await UploadFileAsync(reportModel.UploadEvidence);
                newReport.UploadEvidenceUrl = uploadEvidenceUrl;
            }

            await _reportRepository.CreateReportAsync(newReport);
            await _unitOfWork.SaveChangesAsync();

            return new ReportDto
            {
                UserId = newReport.UserId,
                ReportId = newReport.ReportId,
                DateOccurred = newReport.DateOccurred,
                NameOfTheOffender = newReport.NameOfTheOffender,
                Location = newReport.Location,
                HeightOfTheOffender = newReport.HeightOfTheOffender,
                DidItHappenInYourPresence = newReport.DidItHappenInYourPresence,
                ReportDescription = newReport.ReportDescription,
                UploadEvidenceUrl = newReport.UploadEvidenceUrl,
                ReportStatus = newReport.ReportStatus,
                CategoryID = newReport.CategoryId,
                Message = "Report created successfully",
                Status = true
            };
        }


        public async Task<ReportDto> DeleteReportAsync(Guid reportId)
        {
            var reportToDelete = await _reportRepository.GetReportAsync( r => r.ReportId == reportId);
            if (reportToDelete == null)
            {
                throw new ArgumentException("Report not found");
            }
            _reportRepository.DeleteReportAsync(reportToDelete.ReportId);
            await _unitOfWork.SaveChangesAsync();

            return new ReportDto
            {
                ReportId = reportToDelete.ReportId,
                Message = "Report deleted successfully",
                Status = true,
            };
        }

        public async Task<ICollection<ReportDto>> GetAllReportsAsync()
        {
            var getAllReports = await _reportRepository.GetAllReportsAsync();
            if(getAllReports.Any())
            {
                var report = getAllReports.Select(r => new ReportDto
                {
                    ReportId = r.ReportId,
                    DateOccurred = r.DateOccurred,
                    NameOfTheOffender = r.NameOfTheOffender,
                    Location = r.Location,
                    HeightOfTheOffender = r.HeightOfTheOffender,
                    DidItHappenInYourPresence = r.DidItHappenInYourPresence,
                    ReportDescription = r.ReportDescription,
                    UploadEvidenceUrl = r.UploadEvidenceUrl,
                    ReportStatus = r.ReportStatus,
                    CategoryName = r.Category?.CategoryName,
                    User = r.User,
                    Message = "Report found",
                    Status = true,
                }).ToList();
                return report;
            }

            return new List<ReportDto>();
        }

        public async Task<ReportDto> GetReportByName(string reportName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReportDto>> GetReportsByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ReportDto>> GetAllReportsByUserAsync()
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new UnauthorizedAccessException("User Id not found in claims");
            }

            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                throw new UnauthorizedAccessException("Invalid user ID format");
            }

            var getReportsByUser = await _reportRepository.GetAllReportsAsync(r => r.UserId == parsedUserId);

            if (getReportsByUser == null || !getReportsByUser.Any())
            {
                return new List<ReportDto>();
            }

            var reports = getReportsByUser.Select(r => new ReportDto
            {
                ReportId = r.ReportId,
                DateOccurred = r.DateOccurred,
                NameOfTheOffender = r.NameOfTheOffender,
                Location = r.Location,
                HeightOfTheOffender = r.HeightOfTheOffender,
                DidItHappenInYourPresence = r.DidItHappenInYourPresence,
                ReportDescription = r.ReportDescription,
                UploadEvidenceUrl = r.UploadEvidenceUrl,
                ReportStatus = r.ReportStatus,
                CategoryName = r.Category?.CategoryName,
                User = r.User,
                Message = "Report found",
                Status = true,
            }).ToList();

            return reports;
        }


        public Task<IEnumerable<ReportDto>> SearchReportsAsync(string keyword, DateTime? fromDate, DateTime? toDate)
        {
            throw new NotImplementedException();
        }

        public Task<ReportDto> UpdateReportAsync(UpdateReportModel updateModel)
        {
            throw new NotImplementedException();
        }


        private async Task<string> UploadFileAsync(IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded");
            }

            var allowedFileToUpload = new[] { "image/jpeg", "image/png", "image/gif" };

            var maxFileSize = 5 * 1024 * 1024;

            if (!allowedFileToUpload.Contains(file.ContentType.ToLower()) || file.Length > maxFileSize)
            {
                throw new ArgumentException("Invalid file format. Only jpg, png and gif are allowed and the file length should not exceed 5mb");
            }

            if(string.IsNullOrEmpty(_environment.WebRootPath))
            {
                throw new InvalidOperationException($"Webroot is not configured. Current WebRootPath: {_environment.WebRootPath}");
            }


            var uploadFolderPath = Path.Combine(_environment.WebRootPath, "Uploads");
            if(!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var request = _contextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fileUrl = $"{baseUrl}/Uploads/{uniqueFileName}";
            return fileUrl;

        }
    }


}
