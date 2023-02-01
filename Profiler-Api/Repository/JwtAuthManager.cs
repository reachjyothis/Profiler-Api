using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Profiler_Api.FormModels;
using Profiler_Api.DbModels;

namespace Profiler_Api.Repository;

public class JwtAuthManager: IJwtAuthManager
{
    private readonly IConfiguration _configuration;

    public JwtAuthManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public Response<string> GenerateJwt(User user)
    {
        var response = new Response<string>();

        /* METHOD: 1
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //claim is used to add identity to JWT token
        var claims = new[] {
                 new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                 new Claim("roles", user.Role),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };
        var token = new JwtSecurityToken(_configuration["JWT:Issuer"],
          _configuration["JWT:Audience"],
          claims,
          expires: DateTime.Now.AddMinutes(120),
          signingCredentials: credentials);
        var bearerToken = new JwtSecurityTokenHandler().WriteToken(token);*/

        // METHOD: 2
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("id", user.ID.ToString()),
                new Claim("roles", user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var bearerToken = tokenHandler.WriteToken(token);

        response.Data = "Bearer " + bearerToken;
        response.Code = 200;
        response.Message = "Token generated";
        return response;
    }
}
