using Report_A_Crime.Models.Entities;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Interface
{
    public interface IReportRepository
    {
        Task<Report> CreateReport(Report report);
        Task<Report> GetReportAsync(Expression<Func<Report, bool>> predicate);
        Task UpdateReportAsync(Report report);
        public Task<IEnumerable<Report>> GetAllReportsAsync();
        Task DeleteReportAsync(Guid reportId);
        Task<IEnumerable<Report>> SearchReportsAsync(string searchTerm);    // searching by name or others


    }
}
