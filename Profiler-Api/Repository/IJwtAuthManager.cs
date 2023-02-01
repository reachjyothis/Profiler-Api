using Profiler_Api.FormModels;
using Profiler_Api.DbModels;

namespace Profiler_Api.Repository;

public interface IJwtAuthManager
{
    Response<string> GenerateJwt(User user);
}
