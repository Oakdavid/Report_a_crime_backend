using Report_A_Crime.Models.Entities;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Interface
{
    public interface IReportRepository
    {
        Task<Report> CreateReportAsync(Report report);
        Task<Report> GetReportAsync(Expression<Func<Report, bool>> predicate);
        Task<IEnumerable<Report>> GetAllReportsAsync(Expression<Func<Report, bool>> predicate);
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> FindSimilarReportAsync(Guid categoryId, Guid userId, string reportDescription, DateTime timeFrame); //
        Task<IEnumerable<Report>> GetRecentReportsByUserAsync(Guid userId, DateTime timeFrame);
        Task UpdateReportAsync(Report report);
        Task DeleteReportAsync(Guid reportId);
        Task<IEnumerable<Report>> SearchReportsAsync(string searchTerm);    // searching by name or others
    }
}
