using AuthorApp.DataAccess.DataManagers.Users;
using AuthorApp.DataAccessContracts.Models;
using AuthorApp.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public AuthorizationController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("/token")]
        public async Task<IActionResult> Token(string username, string password)
        {
            var person = await _userManager.GetUser(username, password);
            if (!person.IsSuccess)
                return BadRequest(new { errorText = "Invalid username or password." });

            var identity = GetIdentity(person.Data);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(jwt),
            });
        }

        private ClaimsIdentity GetIdentity(UserDto user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
