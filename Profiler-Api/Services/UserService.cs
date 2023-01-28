using Microsoft.Extensions.Configuration;
using Profiler_Api.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Profiler_Api.Services;

public class UserService: IUserService
{
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public User? GetById(int id)
    {
        //string sql = "SELECT * FROM SomeTable WHERE id IN @ids"
        //var results = conn.Query(sql, new { ids = new[] { 1, 2, 3, 4, 5 } });

        using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        var query = $"SELECT * FROM tbl_users WHERE userid = {id}";
        List<User?> user = db.Query<User>(query, null, commandType: CommandType.Text).ToList()!;
        return user.FirstOrDefault();
    }
}
