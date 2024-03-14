using AutoMapper;
using ChatApp_API.DTOs;
using ChatApp_API.Models;
using ChatApp_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public UserController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterUserDTO userDTO)
        {
            try
            {
                User user = new User
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email
                };

                if (await context.Users.AnyAsync(u => u.UserName == user.UserName || u.Email == user.Email)) return BadRequest("Usuario existente");

                user.Password = HashService.CreateHash(userDTO.Password);

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error occurred" });
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseAuthentication>> Login([FromBody] LoginUserDTO userDTO)
        {
            try
            {
                var userDb = await context.Users.FirstOrDefaultAsync(u => u.UserName == userDTO.UserName);

                if (userDb is null) return NotFound();

                string passwordHash = HashService.CreateHash(userDTO.Password);

                if (userDb.Password != passwordHash) return BadRequest("Contraseña incorrecta");

                return Ok(GenerateToken(userDb));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error occurred" });
            }
        }

        [HttpGet("RefreshToken")]
        public ActionResult<ResponseAuthentication> RefreshToken()
        {
            var claims = HttpContext.User.Claims.ToList();

            var email = claims.Where(c => c.Type == "email").First().Value;
            var userName = claims.Where(c => c.Type == "userName").First().Value;

            return Ok(GenerateToken(new User
            {
                UserName = userName,
                Email = email
            }));
        }

        private ResponseAuthentication GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new("userName", user.UserName),
                new("email", user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credential);

            return new ResponseAuthentication()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                ExpirationTime = expiration
            };
        }

    }
}
