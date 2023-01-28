using System.ComponentModel.DataAnnotations;

namespace Profiler_Api.Models;

public class User
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Role { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}