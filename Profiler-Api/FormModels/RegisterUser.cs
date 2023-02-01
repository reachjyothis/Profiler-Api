using System.ComponentModel.DataAnnotations;

namespace Profiler_Api.FormModels;

public class RegisterUser : LoginModel
{
    [Required]
    public string Email { get; set; } = "";
}
