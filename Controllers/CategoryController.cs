using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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

        [HttpPost]
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
    }
}
