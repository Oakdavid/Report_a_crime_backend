namespace Report_A_Crime.Models.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
        public string? CategoryDescription { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool UpdateCategory { get; set; } = false;   // optional
        ICollection<Report> Reports { get; set; } = new HashSet<Report>();

    }
}
