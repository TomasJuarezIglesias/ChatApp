namespace ChatApp_API.DTOs
{
    public class UserMessageDTO
    {
        public Pagination Pagination { get; set; }
        public UserDTO User { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}
