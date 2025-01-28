namespace Report_A_Crime.Models.Entities
{
    public class Geolocation
    {
        public Guid GeolocationId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? IpAddress {  get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public Guid ReportId { get; set; }
        public Report? Reports { get; set;}
    }
}
