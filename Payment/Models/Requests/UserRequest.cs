using System.ComponentModel.DataAnnotations;

namespace Payment.Models.Requests
{
    public class UserRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
