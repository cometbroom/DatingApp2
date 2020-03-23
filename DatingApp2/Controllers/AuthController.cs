using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp2.Data;
using DatingApp2.Dtos;
using DatingApp2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp2.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _repo;
		private readonly IConfiguration _config;

		public AuthController(IAuthRepository repo, IConfiguration config)
		{
			_config = config;
			_repo = repo;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
		{
			userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

			if (await _repo.UserExists(userForRegisterDto.Username))
				return BadRequest($"User {userForRegisterDto.Username} Already Exists");

			var userToCreate = new User
			{
				Username = userForRegisterDto.Username
			};

			var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

			return StatusCode(201);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
		{
			// throw new Exception("Computer says no");
            //Repository attempts login and returns the user object from database. So use repo for login, register etc...
			var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

			if (userFromRepo == null)
				return Unauthorized();

            //This is the 2 first variables of the token.
			var claims = new[]
			{
                //Type of claim is ID so it's a number as a string.
				new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
				new Claim(ClaimTypes.Name, userFromRepo.Username)
			};

            //Gets the bytes of your token in appsettings.json and makes it as symmetric security key passed to var key
			var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            //Encrypts the key using the security algorithm at the 2nd parameter.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //This will bundle your token information for JWT handler below. It adds subject, expiry date (1 day after) and credentials.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            //Finally create our token.
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //WriteToken() method of token handler will write it in proper form for client
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

            
		}

	}
}