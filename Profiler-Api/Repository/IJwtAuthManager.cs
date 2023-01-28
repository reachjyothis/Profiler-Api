using Dapper;
using Profiler_Api.Models;

namespace Profiler_Api.Repository;

public interface IJwtAuthManager
{
    Response<string> GenerateJwt(User user);
    Response<T> Execute_Command<T>(string query, DynamicParameters spParams);
    Response<List<T>> GetUserList<T>();
    public int? ValidateToken(string? token);
}
