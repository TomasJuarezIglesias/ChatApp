namespace ChatApp_API.DTOs
{
    public class MessageDTO : CreateMessageDTO
    {
        public UserDTO UserSender { get; set; }
        public UserDTO UserReceive { get; set; }
    }
}
