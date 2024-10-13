namespace Report_A_Crime.Models.Entities
{
    public class Geolocation
    {
        public Guid GeolocationId { get; set; }
        public Guid ReportId { get; set; }
        public Report? Reports { get; set;}
    }
}
