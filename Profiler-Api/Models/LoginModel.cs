using System.ComponentModel.DataAnnotations;

namespace Profiler_Api.Models;

public class LoginModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}