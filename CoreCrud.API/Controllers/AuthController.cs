using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreCrud.Services.ICoreCrudService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CoreCrud.API.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IConfiguration _configuration;
		public AuthController(IUserService userService, IConfiguration configuration)
		{
			userService = _userService;
			_configuration = configuration;
		}

		public ActionResult Auth()
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["Keys:SecretKey"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new System.Security.Claims.ClaimsIdentity(
					new Claim[]
					{
						new Claim(ClaimTypes.Role, "Admin")
					}),
				Expires = DateTime.UtcNow.AddDays(1),
				IssuedAt = DateTime.UtcNow,
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha512
				)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return Ok(new { key=tokenHandler.WriteToken(token)});
		}
	}
}
