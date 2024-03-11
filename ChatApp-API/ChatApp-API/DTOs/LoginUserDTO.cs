using System.ComponentModel.DataAnnotations;

namespace ChatApp_API.DTOs
{
    public class LoginUserDTO
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
