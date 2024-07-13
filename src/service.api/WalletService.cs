using model.api;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.KeyStore.Model;
using Nethereum.Signer;
using repository.api.Interfaces;
using service.api.Interfaces;
using shared.api.Dtos.Inputs;
using shared.api.Dtos.Ouputs;
using shared.api.Extensions;
using System.Text;

namespace service.api;

public class WalletService : IWalletService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IBlockchainRepository _blockchainRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IUserAccessor _userAccessor;
    //private readonly IDomainValidation _validation;
    //private readonly IValidator<AddAssetInputDto> _validator;
    public WalletService(IAccountRepository accountRepository,
                         IBlockchainRepository blockchainRepository,
                         IWalletRepository walletRepository,
                         IUserAccessor userAccessor)
    {
        _accountRepository = accountRepository;
        _blockchainRepository = blockchainRepository;
        _walletRepository = walletRepository;
        _userAccessor = userAccessor;
    }

    public async Task Add(AddWalletInputDto dto)
    {
        var blockchain = await _blockchainRepository.GetBlockchainById(dto.BlockchainId.GetValueOrDefault());

        var account = await _accountRepository.GetById(dto.AccountId.GetValueOrDefault());

        var wallet = new Wallet(dto.Name, blockchain.Id, account.Id);

        await _walletRepository.Add(wallet);
    }

    public async Task<AddAssetOutputDto> AddAsset(AddAssetInputDto dto)
    {
        var walett = await _walletRepository.GetWalletById(dto.WalletId);

        //check if wallet belongs to account token requested.

        var ecKey = EthECKey.GenerateKey();
        var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
        var web3Account = new Nethereum.Web3.Accounts.Account(privateKey);

        var seed = BCrypt.Net.BCrypt.GenerateSalt(16);
        var keystoreInBytes = GenerateKeystore(seed, ecKey);

        var asset = new Asset(dto.WalletId, web3Account.Address, keystoreInBytes);

        walett.AddAsset(asset);        
        await _walletRepository.Update(walett);

        return new AddAssetOutputDto { Id = asset.Id, Address = asset.Address, PrivateKey = web3Account.PrivateKey, PublicKey = web3Account.PublicKey, SeedPhrase = seed };
    }

    public async Task<IEnumerable<WalletOutputDto>> GetWalletsByAccount()
    {        
        var accountId = _userAccessor.GetAccountId();
        var wallets = await _walletRepository.GetWalletsByAccount(accountId, true);

        return wallets.Select(wallet => new WalletOutputDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Assets = wallet.Assets?.Select(asset => new AssetOutputDto            
            {
                Id = asset.Id,
                Address = asset.Address,
                Balance = asset.Balance,
            }).ToList()
        }).ToList();
    }

    public static byte[] GenerateKeystore(string seedPhrase, EthECKey eckey)
    {
        var keyStoreService = new Nethereum.KeyStore.KeyStoreScryptService();
        var scryptParams = new ScryptParams { Dklen = 32, N = 262144, R = 1, P = 8 };
        var keyStore = keyStoreService.EncryptAndGenerateKeyStore(seedPhrase, eckey.GetPrivateKeyAsBytes(), eckey.GetPublicAddress(), scryptParams);
        var json = keyStoreService.SerializeKeyStoreToJson(keyStore);
        var keystoreInBytes = Encoding.ASCII.GetBytes(json);

        return keystoreInBytes;
    }
}