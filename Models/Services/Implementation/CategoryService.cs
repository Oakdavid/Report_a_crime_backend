using Report_A_Crime.Models.Dto;
using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Repositories.Implementation;
using Report_A_Crime.Models.Repositories.Interface;
using Report_A_Crime.Models.Services.Interface;
using System.Linq.Expressions;

namespace Report_A_Crime.Models.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(CategoryRepository categoryRepository, IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryRequestModel model)
        {
            bool categoryExistAsync = await _categoryRepository.CategoryExistAsync(c => c.CategoryName == model.CategoryName);
            if(categoryExistAsync)
            {
                return new CategoryDto
                {
                    Message = "Category with this name already Existed",
                };
            }

            var category = new Category
            {
                CategoryName = model.CategoryName,
                CategoryDescription = model.CategoryDescription,
            };
            await _categoryRepository.CreateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = model.CategoryName,
                CategoryDescription = model.CategoryDescription,
                Reports = category.Reports,
                Message = "Category created successfully"
            };
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
           var category = await _categoryRepository.GetCategoryAsync( c => c.CategoryId == categoryId );
            if(category == null || category.IsDeleted)
            {
                return false;
            }
            category.IsDeleted = true;
            await _categoryRepository.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var getAllCategories = await _categoryRepository.GetAllCategoryAsync();
            if(getAllCategories != null && getAllCategories.Any())
            {
                var categories = getAllCategories.Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                }).ToList();
                return categories;
            }
            //return new List<CategoryDto> {new CategoryDto
            //{
            //   Message = "No Category found",
            //   Status = false,
            //}};
            return new List<CategoryDto>();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid categoryId, Category category)
        {
            var existingCategory = await _categoryRepository.GetCategoryAsync(c => c.CategoryId == categoryId);
            if(existingCategory == null)
            {
                return new CategoryDto
                {
                    Message = "Category not found",
                    Status = false
                };
            }
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.CategoryDescription = category.CategoryDescription;
            await _categoryRepository.UpdateCategoryAsync(existingCategory);
            await _unitOfWork.SaveChangesAsync();
            return new CategoryDto
            {
                CategoryId = existingCategory.CategoryId,
                CategoryName = existingCategory.CategoryName,
                CategoryDescription = existingCategory.CategoryDescription,
                Message = "Category updated successfully",
                Status = true
            };
        }
    }
}
