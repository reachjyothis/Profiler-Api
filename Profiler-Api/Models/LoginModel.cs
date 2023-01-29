using System.ComponentModel.DataAnnotations;

namespace Profiler_Api.Models;

public class LoginModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}