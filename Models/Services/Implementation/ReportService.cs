﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interface;
using Report_A_Crime.Models.Repositories.Interphase;
using Report_A_Crime.Models.Services.Interface;
using System.Security.Claims;

namespace Report_A_Crime.Models.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _environment;


        public ReportService(IReportRepository reportRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, ICategoryRepository categoryRepository, IHttpContextAccessor contextAccessor, IWebHostEnvironment environment)
        {
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
           _categoryRepository = categoryRepository;
            _contextAccessor = contextAccessor;
            _environment = environment;
        }

        public async Task<ReportDto> CreateReportAsync(ReportRequestModel reportModel)
        {
            //var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userId = Guid.NewGuid().ToString();
            if(userId == null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            if (reportModel.CategoryId == Guid.Empty)
            {
                return new ReportDto
                {
                    Message = "A valid category must be selected",
                    Data = null,
                    Status = false,
                };
                ////throw new ArgumentException("A valid category must be selected.");
                //reportModel.CategoryId = Guid.Parse("b87c546e-1b45-4112-946d-9d7bbd2b3652");
            }

            var categoryExists = await _categoryRepository.CategoryExistAsync( c => c.CategoryId == reportModel.CategoryId );
            if(!categoryExists)
            {
                return new ReportDto
                {
                    Message = "The specified category does not exist",
                    Status = false,
                };
            }


            var existingReport = await _reportRepository.FindSimilarReportAsync(reportModel.CategoryId, Guid.Parse(userId), reportModel.ReportDescription, DateTime.UtcNow.AddDays(-1));

            if(existingReport != null)
            {
                return new ReportDto
                {
                    Message = "A similar report already exists",
                    Data = null,
                    Status = false,
                };
            }

            var newReport = new Report
            {

                UserId = Guid.Parse(userId),
                CategoryId = reportModel.CategoryId,
                DateOccurred = DateTime.SpecifyKind(reportModel.DateOccurred, DateTimeKind.Utc), // jst added
                CreatedAt = DateTime.UtcNow,
                NameOfTheOffender = reportModel.NameOfTheOffender,
                Location = reportModel.Location,
                HeightOfTheOffender = reportModel.HeightOfTheOffender,
                DidItHappenInYourPresence = reportModel.DidItHappenInYourPresence,
                ReportDescription = reportModel.ReportDescription,
                //UploadEvidenceUrl = reportModel.UploadEvidenceUrl,
                ReportStatus = Enums.ReportStatus.UnderReview,
            };

            if (reportModel.UploadEvidence != null && reportModel.UploadEvidence.Length > 0) 
            {
                var uploadEvidenceUrl = await UploadFileAsync(reportModel.UploadEvidence);
                newReport.UploadEvidenceUrl = uploadEvidenceUrl;
            }
            await _reportRepository.CreateReportAsync(newReport); 
            await _unitOfWork.SaveChangesAsync();

            return new ReportDto
            {
                ReportId = newReport.ReportId,
                DateOccurred = newReport.DateOccurred,
                NameOfTheOffender = newReport.NameOfTheOffender,
                Location = newReport.Location,
                HeightOfTheOffender = newReport.HeightOfTheOffender,
                DidItHappenInYourPresence = newReport.DidItHappenInYourPresence,
                ReportDescription = newReport.ReportDescription,
                UploadEvidenceUrl = newReport.UploadEvidenceUrl,
               ReportStatus = Enums.ReportStatus.UnderReview,
                Category = newReport.Category,
                User = newReport.User,
                Message = "Report created successfully",
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

        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync() // all a specific report from a user
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
                    ReportStatus = Enums.ReportStatus.UnderReview, // remove or leave
                    Category = r.Category,
                    User = r.User,
                    Message = "All report found",
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
            if(userId == null)
            {
                throw new UnauthorizedAccessException("User Id not found in claims");
            }
            var getReportsByUser = await _reportRepository.GetAllReportsAsync(r => r.UserId == new Guid(userId));
            if (getReportsByUser != null || getReportsByUser.Any())
            {
                throw new ArgumentException("No reports found for this user");
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
                ReportStatus = Enums.ReportStatus.UnderReview, // remove or leave
                Category = r.Category,
                User = r.User,
                Message = "All report found",
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
