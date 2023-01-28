using Profiler_Api.Models;

namespace Profiler_Api.Services;

public interface IUserService
{
    User? GetById(int id);
}
