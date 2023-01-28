using Dapper;
using Profiler_Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Profiler_Api.Repository;

public class JwtAuthManager: IJwtAuthManager
{
    private readonly IConfiguration _configuration;
    private readonly string _jwtSecret;

    public JwtAuthManager(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwtSecret = configuration["JWT:Secret"]!;
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
                 new Claim(JwtRegisteredClaimNames.Email, user.Email),
                 new Claim("roles", user.Role),
                 new Claim("Date", DateTime.Now.ToString(CultureInfo.InvariantCulture)),
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
                new Claim("id", user.UserId.ToString()),
                new Claim("roles", user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var bearerToken = tokenHandler.WriteToken(token);

        response.Data = bearerToken;
        response.code = 200;
        response.message = "Token generated";
        return response;
    }

    public Response<T> Execute_Command<T>(string query, DynamicParameters spParams)
    {
        var response = new Response<T>();
        using IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        if (dbConnection.State == ConnectionState.Closed)
            dbConnection.Open();
        using var transaction = dbConnection.BeginTransaction();
        try
        {
            response.Data = dbConnection.Query<T>(query, spParams, commandType: CommandType.StoredProcedure, transaction: transaction).FirstOrDefault() ?? throw new Exception();
            response.code = spParams.Get<int>("retVal"); //get output parameter value
            response.message = "Success";
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            response.code = 500;
            response.message = ex.Message;
        }

        return response;
    }

    public Response<List<T>> GetUserList<T>()
    {
        var response = new Response<List<T>>();
        using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        const string query = "Select userid,username,email,[role],reg_date FROM tbl_users";
        response.Data = db.Query<T>(query, null, commandType: CommandType.Text).ToList();
        return response;
    }

    public int? ValidateToken(string? token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clock-skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            // return user id from JWT token if validation successful
            return userId;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }
}
