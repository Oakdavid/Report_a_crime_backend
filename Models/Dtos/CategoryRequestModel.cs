using Report_A_Crime.Models.Entities;

namespace Report_A_Crime.Models.Dtos
{
    public class CategoryDto : BaseResponse
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
        public string? CategoryDescription { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool UpdateCategory { get; set; } = false; 
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
    }

    public class CategoryRequestModel
    {
        public string CategoryName { get; set; } = default!;
        public string? CategoryDescription { get; set; }

    }

    public class CategoryUpdateModel

    {
        public string CategoryName { get; set; } = default!;
        public string? CategoryDescription { get; set; }
    }
}
