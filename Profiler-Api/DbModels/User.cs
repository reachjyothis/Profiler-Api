using System.ComponentModel.DataAnnotations.Schema;

namespace Profiler_Api.DbModels;

public class User
{
    public int ID { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string PasswordSalt { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Now;
}
