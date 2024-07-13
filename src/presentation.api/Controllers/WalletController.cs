using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.api.Interfaces;
using shared.api.Abstracts;
using shared.api.Dtos.Inputs;
using shared.api.Filters;
using shared.api.Validations.Interfaces;

namespace presentation.api.Controllers;

[ApiController]
[Route("api/wallet")]
public class WalletController : BaseController
{
    private readonly ILogger<AccountController> _logger;
    private readonly IWalletService _walletService;

    public WalletController(ILogger<AccountController> logger,
                            IWalletService walletService,
                            IDomainValidation _validation) : base(_validation)
    {
        _logger = logger;
        _walletService=walletService;
    }

    [HttpPost]    
    public async Task<IActionResult> AddWallet([FromHeader(Name="x-account")]string account, [FromBody] AddWalletInputDto dto)
    {
        _logger.LogInformation("entrypoint add wallet");
        await _walletService.Add(dto);
        return CustomResponse();
    }

    [HttpPost("asset/add")]
    [AllowAnonymous]
    public async Task<IActionResult> AddAsset([FromBody] AddAssetInputDto dto)
    {
        _logger.LogInformation("entrypoint add asset");
        
        return CustomResponse(await _walletService.AddAsset(dto));
    }

    [HttpGet]
    [SampleAuthorization("ADMIN","USER")]
    public async Task<IActionResult> GetWallets()
    {
        _logger.LogInformation("entrypoint get wallets and assets by account");
        
        return CustomResponse(await _walletService.GetWalletsByAccount());
    }
}