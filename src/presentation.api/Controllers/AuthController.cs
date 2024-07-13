using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.api.Interfaces;
using shared.api.Abstracts;
using shared.api.Dtos.Inputs;
using shared.api.Validations.Interfaces;

namespace presentation.api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    public AuthController(ILogger<AccountController> logger, 
                          IAccountService accountService, 
                          IDomainValidation _validation) : base(_validation)
    {
        _logger=logger;
        _accountService=accountService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Verify([FromBody] VerifyAccountInputDto dto)
    {
        var verified = await _accountService.Verify(dto.Email, dto.Password);

        return CustomResponse(verified);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyAndRenew([FromBody] VerifyRefreshInputDto dto)
    {
        var verified = await _accountService.VerifyRefreshAndRenew(dto.RefreshToken);

        return CustomResponse(verified);
    }
}