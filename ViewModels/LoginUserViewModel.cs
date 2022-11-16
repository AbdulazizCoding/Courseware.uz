using System.ComponentModel.DataAnnotations;

namespace courseware.ViewModels;

public class LoginUserViewModel
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Password { get; set; }

    public string? ReturnUrl { get; set; }
}