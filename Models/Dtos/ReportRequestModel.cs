﻿using Report_A_Crime.Models.Entities;
using Report_A_Crime.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Report_A_Crime.Models.Dtos
{
    public class ReportDto : BaseResponse 
    {

        public Guid ReportId { get; set; }
        public DateTime DateOccurred { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? NameOfTheOffender { get; set; }
        public string Location { get; set; } = default!;
        public string? HeightOfTheOffender { get; set; }
        public bool DidItHappenInYourPresence { get; set; }
        public string ReportDescription { get; set; } = default!;
        [NotMapped]
        //public IFormFile? UploadEvidence { get; set; }
        public string? UploadEvidenceUrl { get; set; }
        public ReportStatus ReportStatus { get; set; }
        //public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        //public Guid? UserId { get; set; }
        public User? User { get; set; }
        ICollection<SharedWithUs> SharedWithUs { get; set; } = new HashSet<SharedWithUs>();
        ICollection<Geolocation> Geolocation { get; set; } = new HashSet<Geolocation>();
    }

    public class ReportRequestModel
    {
        public Guid CategoryId { get; set; }
        public DateTime DateOccurred { get; set; } = DateTime.UtcNow;
        public string? NameOfTheOffender { get; set; }
        public string Location { get; set; } = default!;
        public string? HeightOfTheOffender { get; set; }
        public bool DidItHappenInYourPresence { get; set; }
        public string ReportDescription { get; set; } = default!;
        [NotMapped]
        public IFormFile? UploadEvidence { get; set; }
        //public string? UploadEvidenceUrl { get; set; }

    }


}