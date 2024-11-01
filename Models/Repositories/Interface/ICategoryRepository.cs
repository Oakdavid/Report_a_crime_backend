using Report_A_Crime.Models.Entities;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        public Task<bool> CategoryExistAsync(Expression<Func<Category, bool>> predicate);
        Task<Category> GetCategoryAsync(Expression<Func<Category, bool>> predicate);
        Task<IEnumerable<Category>> GetAllCategoryAsync(Expression<Func<Category, bool>> predicate);
        Task<IEnumerable<Category>> GetAllCategoryAsync();
        Task DeleteCategoryAsync(Guid reportId);
        Task<Category> UpdateCategoryAsync(Category category);
    }
}   
