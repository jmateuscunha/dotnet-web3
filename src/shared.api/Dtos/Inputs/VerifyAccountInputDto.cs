using System.ComponentModel.DataAnnotations;

namespace shared.api.Dtos.Inputs;

public class VerifyAccountInputDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}