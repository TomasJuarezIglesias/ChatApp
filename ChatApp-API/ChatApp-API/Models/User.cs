using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChatApp_API.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(256)]
        public string Password { get; set; }
    }
}
