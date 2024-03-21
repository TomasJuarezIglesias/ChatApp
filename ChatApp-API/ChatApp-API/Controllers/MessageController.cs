using AutoMapper;
using ChatApp_API.DTOs;
using ChatApp_API.Models;
using ChatApp_API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/{userToInteract:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageController: ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public MessageController(IMapper mapper, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet()]
        public async Task<ActionResult<UserMessageDTO>> GetAll( [FromRoute] int userToInteract, [FromQuery] int Page = 1, [FromQuery] int Quantity = 50)
        {
            var userName = HttpContext.User.Claims.Where(c => c.Type == "userName").First().Value;

            var userDB = await context.Users.Where(user => user.UserName == userName).FirstAsync();

            var userSendDB = await context.Users.Where(user => user.Id == userToInteract).FirstOrDefaultAsync();

            if (userSendDB is null) return NotFound();

            Pagination paginacion = new(Page, Quantity);

            var queryable = context.Messages.AsQueryable();

            paginacion.Total = queryable
                .Where(message => message.UserSenderId == userDB.Id || message.UserReceiveId == userDB.Id && message.UserSenderId == userSendDB.Id || message.UserReceiveId == userSendDB.Id)
                .Count();

            var messages = await queryable
                .Where(message => message.UserSenderId == userDB.Id || message.UserReceiveId == userDB.Id && message.UserSenderId == userSendDB.Id || message.UserReceiveId == userSendDB.Id)
                .OrderBy(message => message.SendTime)
                .Paginate(paginacion)
                .ToListAsync();

            return new UserMessageDTO
            {
                Pagination = paginacion,
                User = mapper.Map<UserDTO>(userSendDB),
                Messages = mapper.Map<List<MessageDTO>>(messages)
            };
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromRoute] int userToInteract, [FromBody] CreateMessageDTO createMessageDTO)
        {
            try
            {
                if (!await context.Users.AnyAsync(user => user.Id == userToInteract)) return BadRequest();

                var userName = HttpContext.User.Claims.Where(c => c.Type == "userName").First().Value;
                var userDB = await context.Users.Where(user => user.UserName == userName).FirstAsync();

                var message = mapper.Map<Message>(createMessageDTO);
                message.UserReceiveId = userToInteract;
                message.UserSenderId = userDB.Id;

                await context.AddAsync(message);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error occurred" });
            }
        }

    }
}
