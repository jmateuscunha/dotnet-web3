using communication.api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.api.Interfaces;
using shared.api.Abstracts;
using shared.api.Dtos.Inputs;
using shared.api.Validations.Interfaces;

namespace presentation.api.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController : BaseController
{
    private readonly ILogger<AccountController> _logger;
    private readonly ITransactionService _transactionService;
    private readonly IEthereumGrpcClient _grpcClient;

    public TransactionController(ILogger<AccountController> logger,
                                 ITransactionService transactionService,
                                 IDomainValidation _validation,
                                 IEthereumGrpcClient grpcClient) : base(_validation)
    {
        _logger = logger;
        _transactionService=transactionService;
        _grpcClient=grpcClient;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionInputDto dto)
    {
        _logger.LogInformation("entrypoint create transaction");
        
        return CustomResponse(await _transactionService.CreateTransaction(dto));
    }

    [HttpPost("deploy")]
    public async Task<IActionResult> DeploySmartContract([FromBody] DeploySmartContractInputDto dto)
    {
        _logger.LogInformation("entrypoint deploy sc");

        return CustomResponse(await _transactionService.DeploySmartContract(dto));
    }

    [HttpPost("call")]
    public async Task<IActionResult> CallSmartContract([FromBody] CallSmartContractInputDto dto)
    {
        _logger.LogInformation("entrypoint call sc");

        return CustomResponse(await _transactionService.CallSmartContract(dto));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("entrypoint get all transactions");

        return CustomResponse(await _transactionService.GetAll());
    }

    [HttpGet("gas")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPriorityGasPrice(CancellationToken ct)
    {
        _logger.LogInformation("entrypoint get gas prices");

        return CustomResponse(await _transactionService.GetPriorityGasPrices(ct));
    }

    [HttpGet("grpc")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllX()
    {
        _logger.LogInformation("entrypoint get all transactions");

        var resultado = await _grpcClient.GetAlgo();
        return CustomResponse(resultado);
    }
}

