using Dapper;
using Microsoft.AspNetCore.Mvc;
using Profiler_Api.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Profiler_Api.Repository;


namespace Profiler_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IJwtAuthManager _authentication;

    public AccountController(IJwtAuthManager authentication)
    {
        _authentication = authentication;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginModel user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Parameter is missing");
        }

        var dpParam = new DynamicParameters();
        dpParam.Add("email", user.Email, DbType.String);
        dpParam.Add("password", user.Password, DbType.String);
        dpParam.Add("retVal", DbType.String, direction: ParameterDirection.Output);
        var result = _authentication.Execute_Command<User>("sp_loginUser", dpParam);
        if (result.code != 200) return NotFound(result.Data);
        var token = _authentication.GenerateJwt(result.Data);
        return Ok(token);
    }

    [HttpGet("UserList")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllUsers()
    {
        var result = _authentication.GetUserList<User>();
        return Ok(result);
    }

    [HttpPost("Register")]
    [Authorize(Roles = "Admin")]
    public IActionResult Register([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Parameter is missing");
        }
        var dpParam = new DynamicParameters();
        dpParam.Add("email", user.Email, DbType.String);
        dpParam.Add("username", user.Username, DbType.String);
        dpParam.Add("password", user.Password, DbType.String);
        dpParam.Add("role", user.Role, DbType.String);
        dpParam.Add("retVal", DbType.String, direction: ParameterDirection.Output);
        var result = _authentication.Execute_Command<User>("sp_registerUser", dpParam);
        if (result.code == 200)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete("Delete")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(string id)
    {
        if (id == string.Empty)
        {
            return BadRequest("Parameter is missing");
        }

        var dpParam = new DynamicParameters();
        dpParam.Add("userid", id, DbType.String);

        dpParam.Add("retVal", DbType.String, direction: ParameterDirection.Output);
        var result = _authentication.Execute_Command<User>("sp_deleteUser", dpParam);
        if (result.code == 200)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
}
