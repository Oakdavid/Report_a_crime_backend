namespace Report_A_Crime.Models.Dtos
{
    public abstract class BaseResponse
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public string? Data { get; set; }
    }
}
