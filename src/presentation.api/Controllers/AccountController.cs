using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.api.Interfaces;
using shared.api.Abstracts;
using shared.api.Dtos.Inputs;
using shared.api.Validations.Interfaces;

namespace presentation.api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : BaseController
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger,
                             IAccountService accountService,
                             IDomainValidation _validation) : base(_validation)
    {
        _logger = logger;
        _accountService=accountService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddAccount([FromBody] AccountInputDto dto)
    {
        _logger.LogInformation("entrypoint addaccount");
        await _accountService.Add(dto);
        return CustomResponse();
    }
}