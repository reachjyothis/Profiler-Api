using Dapper;
using Profiler_Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Profiler_Api.Repository;

public class JwtAuthManager: IJwtAuthManager
{
    private readonly IConfiguration _configuration;

    public JwtAuthManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public Response<string> GenerateJWT(User user)
    {
        var response = new Response<string>();

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
          _configuration["JWT:Issuer"],
          claims,    //null original value
          expires: DateTime.Now.AddMinutes(120),
          signingCredentials: credentials);

        response.Data = new JwtSecurityTokenHandler().WriteToken(token); //return access token
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

    public Response<List<T>> getUserList<T>()
    {
        var response = new Response<List<T>>();
        using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("default"));
        const string query = "Select userid,username,email,[role],reg_date FROM tbl_users";
        response.Data = db.Query<T>(query, null, commandType: CommandType.Text).ToList();
        return response;
    }
}
