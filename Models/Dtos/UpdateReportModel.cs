using System.ComponentModel.DataAnnotations.Schema;

namespace Report_A_Crime.Models.Dtos
{
    public class UpdateReportModel
    {
        public string? NameOfTheOffender { get; set; }
        public string Location { get; set; } = default!;
        public string? HeightOfTheOffender { get; set; }
        public bool DidItHappenInYourPresence { get; set; }
        public string ReportDescription { get; set; } = default!;
        [NotMapped]
        public IFormFile? UploadEvidence { get; set; }
        public string? UploadEvidenceUrl { get; set; }
    }
}
