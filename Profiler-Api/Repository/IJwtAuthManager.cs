using Dapper;
using Profiler_Api.Models;

namespace Profiler_Api.Repository;

public interface IJwtAuthManager
{
    Response<string> GenerateJWT(User user);
    Response<T> Execute_Command<T>(string query, DynamicParameters sp_params);
    Response<List<T>> getUserList<T>();
}
