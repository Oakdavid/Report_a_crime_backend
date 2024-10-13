using Microsoft.EntityFrameworkCore;
using Report_A_Crime.Context;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interface;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ReportCrimeDbContext _dbContext;

        public RoleRepository(ReportCrimeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            await _dbContext.AddAsync(role);
            return role;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            var allRoles = await _dbContext.Roles
                .Include(r => r.Users)
                .ToListAsync();
            return allRoles;
        }

        public async Task<Role> GetRoleAsync(Expression<Func<Role, bool>> expression)
        {
            var role = await _dbContext.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(expression);

            return role;
        }

        public void Remove(Role role)
        {
            _dbContext.Roles .Remove(role);
        }
    }
}
