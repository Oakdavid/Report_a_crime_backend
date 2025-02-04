using Microsoft.EntityFrameworkCore;
using Report_A_Crime.Context;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Interface;
using System;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ReportCrimeDbContext _dbContext;

        public CategoryRepository(ReportCrimeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CategoryExistAsync(Expression<Func<Category, bool>> predicate)
        {
           
            var exist = await _dbContext.Categories.AnyAsync(predicate);
            
            return exist;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _dbContext.AddAsync(category);
            return category;
        }

        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            var deleteCategory = await _dbContext.Categories.FindAsync(categoryId);
            if(deleteCategory != null)
            {
                _dbContext.Categories.Remove(deleteCategory);
            }
                
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync(Expression<Func<Category, bool>> predicate)
        {
            var getAlCategory = await _dbContext.Categories
                .Include(c => c.Reports)
                .Where(predicate)
                .ToListAsync();
            return getAlCategory;

        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            var getAllCategories = await _dbContext.Categories
                .Include (c => c.Reports)
                .ToListAsync();
            return getAllCategories;
        }

        public async Task<Category> GetCategoryAsync(Expression<Func<Category, bool>> predicate)
        {
            var getCategories = await _dbContext.Categories
                .Include(c => c.Reports)
                .FirstOrDefaultAsync(predicate);
            return getCategories;
                
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
           _dbContext.Set<Category>().Update(category);
            return category;
        }
    }
}
