
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UMS.Application.Services
{
    public class JwtTokenHelper
    {
        private readonly string _key;

        public JwtTokenHelper(string key)
        {
            _key = key;
        }

        public string GenerateToken(string username, int userId, string firstName, string lastName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // User ID
                new Claim("firstname", firstName), // Custom claim for first name
                new Claim("lastname", lastName), // Custom claim for last name
            }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Return the generated JWT token
        }
    }

}
