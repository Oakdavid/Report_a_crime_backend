using Report_A_Crime.Models.Dto;
using Report_A_Crime.Models.Dtos;

namespace Report_A_Crime.Models.Services.Interface
{
    public interface IReportService
    {
        public Task<ReportDto> CreateReportAsync(ReportRequestModel reportModel);
        public Task<IEnumerable<ReportDto>> GetAllReportsAsync();
        public Task<ReportDto> GetReportByName(string reportName);
        public Task<ReportDto> UpdateReportAsync(UpdateReportModel updateModel);
        public Task<ReportDto> DeleteReportAsync(Guid reportId);
        public Task<IEnumerable<ReportDto>> SearchReportsAsync(string keyword, DateTime? fromDate, DateTime? toDate); // still thinking on this
        public Task<IEnumerable<ReportDto>> GetReportsByCategoryAsync(string category); // thinking 
        Task<IEnumerable<ReportDto>> GetAllReportsByUserAsync();
    }
}
