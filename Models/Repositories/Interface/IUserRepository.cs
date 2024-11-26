using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Implementation;
using Report_A_Crime.Models.Repositories.Interface;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Interphase
{
    public interface IUserRepository : IBaseRepository<UserRepository>
    {
        public Task<User> CreateUserAsync(User user);
        public Task<User> Update(User user);
        public Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate);
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<bool> UserExistAsync(Expression<Func<User, bool>> predicate);
    }
}
