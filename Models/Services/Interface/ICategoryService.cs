using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Entities;

namespace Report_A_Crime.Models.Services.Interface
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(CategoryRequestModel model);
        Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> UpdateCategoryAsync(Guid categoryId, Category category);
        Task<bool> DeleteCategoryAsync(Guid categoryId);
    }
}
