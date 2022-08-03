using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationDemo.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration??throw new ArgumentNullException(nameof(configuration));
        }

        public class AuthRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        public class RequestedUserInfo
        {
            public int UserId { get; set; }
            public string? UserName { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? City { get; set; }

            public RequestedUserInfo(int userId, string? userName, string? firstName, string? lastName, string? city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }

        [HttpPost("authenticate")]
        public ActionResult<string>Authentication(AuthRequestBody authRequestBody)
        {
            var user = ValidateUserCredentials(authRequestBody.UserName, authRequestBody.Password);
            if(user == null)
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AuthenticationDemo:SecretForKey"]));
            var signingCredntials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["AuthenticationDemo:Issuer"],
                _configuration["AuthenticationDemo:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredntials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private RequestedUserInfo ValidateUserCredentials(string? userName, string? password)
        {
            return new RequestedUserInfo(1, "Mr.", userName, "Lokhande", "Pune");
        }
    }
}
