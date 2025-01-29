using Report_A_Crime.Models.Entities;

namespace Report_A_Crime.Models.Dtos
{
    public class CategoryDto : BaseResponse
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
        public string? CategoryDescription { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<ReportDto> Reports { get; set; } = new List<ReportDto>();
    }

    public class CategoryRequestModel
    {
        public string CategoryName { get; set; } = default!;
        public string? CategoryDescription { get; set; }

    }

    public class CategoryUpdateModel

    {
        public Guid CategoryId { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public string? CategoryDescription { get; set; }
    }
}
