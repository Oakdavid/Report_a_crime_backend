using Report_A_Crime.Context;
using Report_A_Crime.Migrations;
using Report_A_Crime.Models.Repositories.Interface;

namespace Report_A_Crime.Models.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReportCrimeDbContext _dbContext;

        public UnitOfWork(ReportCrimeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
