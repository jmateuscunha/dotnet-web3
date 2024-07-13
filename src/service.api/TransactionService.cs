using communication.api.Interfaces;
using Microsoft.Extensions.Logging;
using model.api;
using Nethereum.Hex.HexTypes;
using Nethereum.KeyStore;
using Nethereum.Web3;
using repository.api.Interfaces;
using service.api.Interfaces;
using shared.api.Dtos;
using shared.api.Dtos.Inputs;
using shared.api.Dtos.Ouputs;
using shared.api.Enums;
using shared.api.Validations.Interfaces;
using System.Numerics;
using System.Text;

namespace service.api;

public class TransactionService : ITransactionService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IDomainValidation _validation;
    private readonly ApiConfiguration _configuration;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IEthereumClient _ethereumClient;
    public readonly ILogger<TransactionService> _logger;
    public TransactionService(IWalletRepository walletRepository,
                              IDomainValidation validation,
                              ApiConfiguration configuration,
                              ITransactionRepository transactionRepository,
                              ILogger<TransactionService> logger,
                              IEthereumClient ethereumClient)
    {
        _walletRepository=walletRepository;
        _validation=validation;
        _configuration=configuration;
        _transactionRepository=transactionRepository;
        _logger=logger;
        _ethereumClient=ethereumClient;
    }
    public async Task<CreateTransactionOutputDto> CreateTransaction(CreateTransactionInputDto dto)
    {
        var asset = await _walletRepository.GetAssetById(dto.FromAssetId);

        var web3 = InitWeb3Account(dto.SeedPhrase, dto.PrivateKey, asset);

        var balance = await web3.Eth.GetBalance.SendRequestAsync(asset.Address);
        var amountRequested = Web3.Convert.ToWei(dto.Amount);
        var remainingBalance = BigInteger.Subtract(balance, amountRequested);

        try
        {
            var (gasPriceInGwei, gasPriceInWei) = await GetIntendedGasPrice(dto.Priority, web3);

            if (balance.Equals(0) || remainingBalance <= 0 || BigInteger.Subtract(remainingBalance, gasPriceInWei) <= 0)
            {
                _validation.AddError("Insufficient funds to transfer.");
                return null;
            }

            var nextNonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(asset.Address);
            var transactionHash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(dto.DestinationAddress,etherAmount: dto.Amount, gasPriceInGwei, null, nextNonce);
            var transaction = new Transaction(transactionHash, asset.Address, dto.DestinationAddress, amountRequested, feeApplied: gasPriceInWei, nextNonce.Value, asset.Id);
            await _transactionRepository.Add(transaction);

            return new CreateTransactionOutputDto { Id = transaction.Id, Hash = transaction.Hash };
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.InnerException?.Message);
            _validation.AddError("Unable to create transaction.");
            return null;
        }
    }

    public async Task<GasPriceSuggestionOutputDto> GetPriorityGasPrices(CancellationToken ct)
    {
        return await _ethereumClient.GetPriorityGasPrices(ct);
    }

    public async Task<IEnumerable<TransactionOutputDto>> GetAll()
    {
        var transactions = await _transactionRepository.GetAll();

        return transactions?.Select(tx => new TransactionOutputDto
        {
            Id = tx.Id,
            Hash = tx.Hash,
            AmountRequested = tx.AmountRequested,
            Nonce = tx.Nonce,
        }).ToList();
    }

    public async Task<object> DeploySmartContract(DeploySmartContractInputDto dto)
    {
        var asset = await _walletRepository.GetAssetById(dto.AssetId);
        var web3 = InitWeb3Account(dto.SeedPhrase, string.Empty, asset);
        var estimateGas = await web3.Eth.DeployContract.EstimateGasAsync(dto.Abi, dto.Bytecode, asset.Address);
        var receipt = await web3.Eth.DeployContract.SendRequestAsync(dto.Bytecode, asset.Address,estimateGas);

        Console.WriteLine("Contract deployed at address: " + receipt);

        return receipt;
    }

    public async Task<object> CallSmartContract(CallSmartContractInputDto dto)
    {
        var asset = await _walletRepository.GetAssetById(dto.AssetId);
        var web3 = InitWeb3Account(dto.SeedPhrase, string.Empty, asset);
        var contract = web3.Eth.GetContract(dto.Abi, dto.ContractAddress);

        var functionOutput = await contract.GetFunction(dto.FunctionName).CallAsync<object>();

        return functionOutput;
    }

    private Web3 InitWeb3Account(string seedPhrase, string privateKey, Asset asset)
    {
        if (!string.IsNullOrEmpty(seedPhrase))
        {
            var bpvk = new KeyStoreScryptService().DecryptKeyStoreFromJson(seedPhrase, Encoding.ASCII.GetString(asset.Keystore));
            return new Web3(new Nethereum.Web3.Accounts.Account(bpvk, Nethereum.Signer.Chain.Sepolia), _configuration.SepoliaRPC);
        }

        var account = new Nethereum.Web3.Accounts.Account(privateKey, Nethereum.Signer.Chain.Sepolia);

        return new Web3(account, _configuration.SepoliaRPC);
    }

    private async Task<(decimal,BigInteger)> GetIntendedGasPrice(TransactionPriority priority, Web3 web3)
    {
        var unitValue = Web3.Convert.GetEthUnitValue(Nethereum.Util.UnitConversion.EthUnit.Gwei);

        var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        var fees = await web3.Eth.FeeHistory.SendRequestAsync(new HexBigInteger(1), new Nethereum.RPC.Eth.DTOs.BlockParameter(latestBlock), [25, 75, 99]);
        var sugestedGasPriceInWei = await web3.Eth.GasPrice.SendRequestAsync();

        _logger.LogInformation($"Suggested GasPrices in Wei: Low={fees.Reward[0][0]} Medium={fees.Reward[0][1]} High={fees.Reward[0][2]}");
        _logger.LogInformation($"Suggested Default GasPrice: {sugestedGasPriceInWei}");

        switch (priority)
        {
            case TransactionPriority.LOW: return (Web3.Convert.FromWei(fees.Reward[0][0], unitValue), fees.Reward[0][0]);                
            case TransactionPriority.MEDIUM: return (Web3.Convert.FromWei(fees.Reward[0][1], unitValue), fees.Reward[0][1]);
            case TransactionPriority.HIGH: return (Web3.Convert.FromWei(fees.Reward[0][2], unitValue), fees.Reward[0][2]);
            default:
                {
                    var gasPriceInGwei = Web3.Convert.FromWei(sugestedGasPriceInWei, unitValue);
                    return (gasPriceInGwei, sugestedGasPriceInWei);
                }
        }
    }
}