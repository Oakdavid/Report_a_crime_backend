using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Report_A_Crime.Exception;
using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Services.Interface;

namespace Report_A_Crime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CategoryController(IReportService reportService, ICategoryService categoryService, IHttpContextAccessor contextAccessor)
        {
            _reportService = reportService;
            _categoryService = categoryService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody]CategoryRequestModel model)
        {
            var category = await _categoryService.CreateCategoryAsync(model);
            if(!category.Status)
            {
                return BadRequest(new
                {
                    Message = category.Message,
                    Status = false,
                }) ;
            }

            return Ok(new
            {
                Data = category
            });
        }

        [HttpGet("AllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {

            var allCategory = await _categoryService.GetAllCategoriesAsync();
            if(allCategory != null && allCategory.Any())
            {
                return Ok(new
                {
                    data = allCategory
                });
            }
            return NotFound(new
            {
                Message = "No category found",
                Status = false,
            });
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromBody] Guid categoryId)
        {

           try
           {
                var deleteCategory = await _categoryService.DeleteCategoryAsync(categoryId);
                if(deleteCategory != null)
                {
                    return Ok(new
                    {
                        Message = " Category delete successfully",
                        Status = true
                    });
                }
                return NotFound(new
                {
                    Message = "Category not found or already deleted",
                    Status = false
                });

           }
           catch (System.Exception ex)
           {
                return StatusCode(500, new
                {
                    Message = "An error occurred while attempting to delete the category",
                    Status = false,
                });
           }
        }

        [HttpPut("CategoryId")]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] CategoryUpdateModel model)
        {
            if(categoryId == Guid.Empty)
            {
                return BadRequest("CategoryId is required");
            }

            if(model == null)
            {
                return BadRequest("Category data is required");
            }

            var update = await _categoryService.UpdateCategoryAsync(categoryId,model);

            if(update == null)
            {
                return NotFound("Category not found");
            }

            if(update.Status)
            {
                return Ok(new
                {
                    Data = update
                });
            }
            return NotFound(new
            {
                Messag = update.Message
            });
        }
    }
}
