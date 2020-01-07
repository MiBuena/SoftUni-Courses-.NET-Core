using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    public class AuthenticationController : BaseApiController
    {
        private readonly JwtSettings _jwtSettings;

        public AuthenticationController(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        [HttpGet("[action]")]
        public ActionResult<string> WhoAmI()
        {

            return "Who am I" + this.User.Identity.Name;
        }

        [HttpGet("[action]")]
        [Authorize]
        public ActionResult<string> WelcomeAdmin()
        {

            var a = this.User.Identity;

            var b = this.User.Identity.IsAuthenticated;
            return "Welcome, Admin";
        }

        [HttpGet("[action]")]
        public ActionResult<string> Login(string username, string password)
        {
            if(username == "admin" && password == "admin")
            {
                var secret = _jwtSettings.Secret;

                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow.AddDays(7),
                    Subject = new System.Security.Claims.ClaimsIdentity(new []
                    {
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(ClaimTypes.Name, "admin@ab.b")
                    }),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var jwt = tokenHandler.WriteToken(token);

                return jwt;
            }

            return this.Forbid();
        }
    }
}
