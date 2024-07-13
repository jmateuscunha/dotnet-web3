using System.ComponentModel.DataAnnotations;

namespace shared.api.Dtos.Inputs;

public class AccountInputDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(8, ErrorMessage = "Password must contain at least 8 characters length.")]
    public string Password { get; set; }
    public string Role { get; set; }
}
