using Microsoft.EntityFrameworkCore;
using Report_A_Crime.Context;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interface;
using System.Composition;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Implementation
{
    public class ReportRepository : BaseRepository<Report>, IReportRepository
    {
        private readonly ReportCrimeDbContext _dbContext;

        public ReportRepository(ReportCrimeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Report> CreateReportAsync(Report report)
        {
            await _dbContext.Reports.AddAsync(report);
            return report;
        }

        public async Task DeleteReportAsync(Guid reportId)
        {
            var deleteReport = await _dbContext.Reports.FindAsync(reportId);
            if(deleteReport != null)
            {
                _dbContext.Reports.Remove(deleteReport);    // recheck this
            }
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            var reports = await _dbContext.Reports
                .Include(r => r.User)
                .Include(r => r.Category)
                .ToListAsync();
            return reports;
        }


        public async Task<Report> GetReportAsync(Expression<Func<Report, bool>> predicate)
        {
            var report = await _dbContext.Reports
                .Include(r => r.User)
                .Include(r => r.Category)
                .FirstOrDefaultAsync(predicate);
            return report;
        }
        public async Task<IEnumerable<Report>> GetRecentReportsByUserAsync(Guid userId, DateTime timeFrame)
        {
            return await _dbContext.Reports
                .Where(r => r.UserId == userId
                && r.CreatedAt >= timeFrame)
                .ToListAsync();
        }

        public async Task<Report> FindSimilarReportAsync(string categoryName, Guid userId, string reportDescription, DateTime timeFrame)
        {
            var getSimilarReportAsync =  await _dbContext.Reports
                .Where(r => r.Category.CategoryName == categoryName
                && r.UserId == userId
                && r.ReportDescription == reportDescription
                && r.CreatedAt >= timeFrame)
                .Include(r => r.User)
                .Include(r => r.Category)
                .FirstOrDefaultAsync();
            return getSimilarReportAsync;
        }

        public async Task<IEnumerable<Report>> SearchReportsAsync(string searchTerm)
        {
            var searchReport = await _dbContext.Reports
                .Include(r => r.User)
                .Include(r => r.Category)
                .Where(r => r.ReportDescription !=null && r.ReportDescription.Contains(searchTerm.ToLower()) ||
                        r.NameOfTheOffender != null && r. NameOfTheOffender.Contains(searchTerm.ToLower()) ||
                        r.Location != null && r.Location.Contains(searchTerm.ToLower()))
                .ToListAsync();

            return searchReport;
        }

        public async Task UpdateReportAsync(Report report)
        {
            _dbContext.Reports.Update(report);
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync(Expression<Func<Report, bool>> predicate)
        {
            var getAllReports = await _dbContext.Reports
                .Include(r => r.User)
                .Include(r => r.Category)
                .Where(predicate).ToListAsync();
            return getAllReports;
        }
    }
}
