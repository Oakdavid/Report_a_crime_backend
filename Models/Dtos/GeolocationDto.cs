using Report_A_Crime.Models.Entities;

namespace Report_A_Crime.Models.Dtos
{
    public class GeolocationDto : BaseResponse
    {
        public Guid GeolocationId { get; set; }
        public string? IpAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? City { get; set; }
        public Guid ReportId { get; set; }
    }

    public class GeolocationRequestModel
    {
        //public string IpAddress { get; set; }
        //public Guid ReportId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? City { get; set; }
    }
}
