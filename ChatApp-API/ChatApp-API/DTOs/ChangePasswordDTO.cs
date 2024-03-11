using System.ComponentModel.DataAnnotations;

namespace ChatApp_API.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string ActualPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
