using AutoMapper;
using ChatApp_API.DTOs;
using ChatApp_API.Models;

namespace ChatApp_API.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<CreateMessageDTO, Message>();

            CreateMap<Message, MessageDTO>();
        }
    }
}
