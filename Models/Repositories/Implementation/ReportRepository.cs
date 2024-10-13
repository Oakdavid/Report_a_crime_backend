using Microsoft.EntityFrameworkCore;
using Report_A_Crime.Context;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interface;
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

        public async Task<Report> CreateReport(Report report)
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

        public async Task<IEnumerable<Report>> SearchReportsAsync(string searchTerm)
        {
            var searchReport = await _dbContext.Reports
                .Include(r => r.User)
                .Include(r => r.Category)
                .Where(r => r.ReportDescription.Contains(searchTerm) ||
                        r.NameOfTheOffender.Contains(searchTerm) ||         // i might need to recheck this for null reference
                        r.Location.Contains(searchTerm))
                .ToListAsync();

            return searchReport;
        }

        public async Task UpdateReportAsync(Report report)
        {
            _dbContext.Reports.Update(report);
        }
    }
}
