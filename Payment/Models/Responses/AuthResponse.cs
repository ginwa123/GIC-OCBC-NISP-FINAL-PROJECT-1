namespace Payment.Models
{
    public class AuthResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
