using System.ComponentModel.DataAnnotations;

namespace ChatApp_API.DTOs
{
    public class RegisterUserDTO : LoginUserDTO
    {
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }
    }
}
