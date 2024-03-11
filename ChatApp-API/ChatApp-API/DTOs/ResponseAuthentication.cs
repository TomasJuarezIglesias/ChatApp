namespace ChatApp_API.DTOs
{
    public class ResponseAuthentication
    {
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
