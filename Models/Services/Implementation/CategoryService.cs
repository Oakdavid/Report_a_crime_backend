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

        public CategoryService(ICategoryRepository categoryRepository, IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
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
                var categoryWhichExisted = await _categoryRepository.GetCategoryAsync(a => a.CategoryName == model.CategoryName);
                if(categoryWhichExisted != null && categoryWhichExisted.IsDeleted)
                {
                    categoryWhichExisted.CategoryDescription = model.CategoryDescription;
                    categoryWhichExisted.IsDeleted = false;
                    var newCategory = _categoryRepository.UpdateCategoryAsync(categoryWhichExisted);
                    var changes =  await _unitOfWork.SaveChangesAsync();
                    if(changes > 0)
                        return new CategoryDto
                        {
                            Message = "Successfull",
                            Status = true
                        };
                }
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
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                Reports = category.Reports.Select(report => new ReportDto
                {
                    ReportId = report.ReportId,
                    ReportDescription = report.ReportDescription,
                    DateOccurred = DateTime.Now,
                    Location = report.Location,
                    CreatedAt = DateTime.Now,

                }).ToList(),
                Message = "Category created successfully",
                Status = true
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
                var categories = getAllCategories.Where( c => !c.IsDeleted).Select( c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    Status = true
                }).ToList();
                return categories;
            }

            return new List<CategoryDto>();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid categoryId, CategoryUpdateModel category)
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
