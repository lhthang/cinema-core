using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using cinema_core.Form;
using cinema_core.Repositories;
using cinema_core.Utils.JWT;
using cinema_core.Utils.Password;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace cinema_core.Controllers
{
    [Route("/api/")]
    [ApiController]
    public class LoginController : Controller
    {
        private IUserRepository userRepository;

        private PasswordHasher passwordHasher;

        private JwtToken jwtToken;

        public LoginController(IUserRepository repository)
        {
            userRepository = repository;
            passwordHasher = new PasswordHasher();
            jwtToken = new JwtToken();
        }
        [HttpPost("login")]
        public IActionResult PostUser([FromBody] LoginRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var found = userRepository.GetUser(user.Username);

            if (found == null)
                return Unauthorized();

            if (!passwordHasher.PasswordMatches(user.Password, found.Password))
                return Unauthorized();

            var result = new JwtSecurityTokenHandler().WriteToken(jwtToken.GenerateToken(userRepository, found));
            return Ok(new { token = result });
        }


        [HttpPost("check-token")]
        public IActionResult CheckToken([FromBody] CheckTokenRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(request.Token, validationParameters, out validatedToken);
            var result = principal.Identity == null ? false : true;
            //var token = new JwtSecurityTokenHandler().WriteToken(jwtToken.GenerateToken(userRepository, found));
            return Ok(new
            {
                isValid = result
            });
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "uit-cinema",
                ValidAudience = "user",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-uit-secret-key")) // The same key as the one that generate the token
            };
        }
    }
}