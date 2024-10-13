namespace Report_A_Crime.Exception
{
    public class UserNotFoundException : System.Exception
    {
        public int StatusCode { get; }
        public UserNotFoundException(string message, int statusCode) : base (message)
        {
            StatusCode = statusCode;
        }
    }
}
