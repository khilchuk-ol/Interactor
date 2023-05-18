using System.ComponentModel.DataAnnotations;

namespace SplitDivider.WebApp.Authorization.Models;

public class RegistrationModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
 
    [Required]
    [Compare("Password", ErrorMessage = "passwords mismatch")]
    [DataType(DataType.Password)]
    public string PasswordConfirmation { get; set; }
}