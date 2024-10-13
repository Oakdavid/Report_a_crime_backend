using Microsoft.EntityFrameworkCore;
using Report_A_Crime.Context;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interphase;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Implementation
{
    public class UserRepository : BaseRepository<UserRepository>, IUserRepository
    {
        private readonly ReportCrimeDbContext _dbContext;

        public UserRepository(ReportCrimeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
           await _dbContext.AddAsync(user);
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
           var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.SharedWithUs)
                .Include(u => u.Reports)
                .Include(u => u.RequestAServices)
                .ToListAsync();
            return user;
        }

        public async Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.SharedWithUs)
                .Include(u => u.Reports)
                .Include(u => u.RequestAServices)
                .FirstOrDefaultAsync(predicate);
            return user;
        }

        public async Task<User> Update(User user)
        {
            _dbContext.Set<User>().Update(user);
            return user;
        }

        public async Task<bool> UserExistAsync(Expression<Func<User, bool>> predicate)
        {
            var exist = await _dbContext.Users.AnyAsync(predicate);

            return exist;
        }
    }
}
