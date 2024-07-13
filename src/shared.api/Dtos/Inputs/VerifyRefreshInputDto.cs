using System.ComponentModel.DataAnnotations;

namespace shared.api.Dtos.Inputs;

public class VerifyRefreshInputDto 
{
    [Required]
    public string RefreshToken { get; set; }
}