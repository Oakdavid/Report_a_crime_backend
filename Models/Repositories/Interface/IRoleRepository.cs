using Report_A_Crime.Models.Entities;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Interface
{
    public interface IRoleRepository
    {
        public Task<Role> CreateAsync(Role role);
        public Task<IEnumerable<Role>> GetAllRolesAsync();
        public Task<Role> GetRoleAsync(Expression<Func<Role, bool>> expression);
        public void Delete(Role role);
        public Task<bool> RoleExistAsync(Expression<Func<Role, bool>> predicate);

    }
}
