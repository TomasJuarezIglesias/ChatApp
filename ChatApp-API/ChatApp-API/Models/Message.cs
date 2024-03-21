using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace ChatApp_API.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int UserSenderId { get; set; }
        public int UserReceiveId { get; set; }
        public string Text { get; set; }
        public DateTime SendTime { get; set; }

        // Navigation Properties
        public User UserSender { get; set; }
        public User UserReceive { get; set; }
    }
}
