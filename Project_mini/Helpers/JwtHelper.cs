using Microsoft.IdentityModel.Tokens;
using Project_mini.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_mini.Helpers
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _issuerSigningKey;

        // Constructor to initialize IConfiguration and SigningKey
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            
            // Initialize the SymmetricSecurityKey from the secret key in the configuration
            var key = _configuration["Jwt:Key"];
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
        
        // Method to generate JWT token
        public string GenerateToken(User user)
        {
            // Signing credentials using HmacSha256 algorithm
            var credentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);

            // Claims to be included in the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Subject claim (email)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
                new Claim("role", user.Role.Name) // Role claim (this must match what the server expects)
            };

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],   // Issuer (must match the server configuration)
                audience: _configuration["Jwt:Audience"], // Audience (must match the server configuration)
                claims: claims, // Claims to include in the token
                expires: DateTime.Now.AddMinutes(120), // Token expiration (2 hours in this case)
                signingCredentials: credentials // Signing credentials
            );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
