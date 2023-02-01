using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Profiler_Api.Repository;
using Profiler_Api.FormModels;
using Profiler_Api.DbModels;
using Profiler_Api.Data;

namespace Profiler_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IJwtAuthManager _authentication;
    private readonly SchedulerContext _db;

    public AccountController(IJwtAuthManager authentication, SchedulerContext db)
    {
        _authentication = authentication;
        _db = db;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginModel user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Parameter is missing");
        }

        var dbUser = this._db.Users.Where(s => s.Username == user.UserName).FirstOrDefault<User>();
        if (dbUser == null)
        {
            return BadRequest("Invalid username/password");
        }


        string passwordHash = Utils.Utils.HashPassword(user.Password, dbUser.PasswordSalt, false, out var genSalt);
        if (dbUser.PasswordHash != passwordHash)
        {
            return BadRequest("Invalid username/password");
        }

        var token = _authentication.GenerateJwt(dbUser);
        return Ok(token);
    }

    [HttpGet("UserList")]
    [Authorize(Roles = "A")]
    public IActionResult GetAllUsers()
    {
        var allUsers = this._db.Users.ToList();
        return Ok(allUsers);
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public IActionResult Register([FromBody] RegisterUser user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Parameter is missing");
        }

        string passwordSalt;
        string passwordHash = Utils.Utils.HashPassword(user.Password, "", true, out passwordSalt);
        this._db.Users.Add(new User
        {
            Username = user.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Email = user.Email,
            Role = "U",
            Date = DateTime.Now
        });
        this._db.SaveChanges();
        return Ok("Created.");
    }

    [HttpDelete("Delete")]
    [Authorize(Roles = "A")]
    public IActionResult Delete(int ID)
    {
        var user = this._db.Users.Where(u => u.ID == ID).FirstOrDefault<User>();
        if (user == null)
        {
            return BadRequest("Invalid user ID");
        }
        if (user.ID == 1)
        {
            return BadRequest("This is the main Admin - please delete from the backend.");
        }

        this._db.Users.Remove(user);
        this._db.SaveChanges();
        return Ok("Deleted.");
    }
}
