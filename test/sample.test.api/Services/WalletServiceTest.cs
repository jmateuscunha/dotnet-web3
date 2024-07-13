using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using model.api;
using Moq;
using Moq.AutoMock;
using repository.api.Interfaces;
using sample.test.api.Fakers;
using service.api;
using service.api.Interfaces;
using shared.api.Extensions;

namespace sample.test.api.Services;

public class WalletServiceTest
{
    private readonly AutoMocker _mocker;
    private readonly IWalletService _walletService;
    public WalletServiceTest()
    {
        _mocker = new AutoMocker();
        _walletService = _mocker.CreateInstance<WalletService>();
    }

    [Trait("Service", "Wallet")]
    [Fact(DisplayName = "Should return wallets")]
    public async Task GetWallets()
    {
        //Arrange
        var accountDto = AccountDtoMock.ValidAccountDtoFaker.Generate();
        var role = RoleMock.ValidRoleFaker.Generate();
        var wallets = WalletMock.ValidWalletFaker.Generate(2);
        _mocker.GetMock<IUserAccessor>().Setup(c => c.GetAccountId()).Returns(1);
        _mocker.GetMock<IWalletRepository>().Setup(c => c.GetWalletsByAccount(It.IsAny<int>(),It.IsAny<bool>())).ReturnsAsync(wallets);

        //Act
        var result = await _walletService.GetWalletsByAccount();

        //Assert
        Assert.NotEmpty(result);
    }

    [Trait("Service", "Wallet")]
    [Fact(DisplayName = "Invalid - Wallet not found")]
    public async Task GetWallets_Invalid_NotFound()
    {
        //Arrange
        _mocker.GetMock<IUserAccessor>().Setup(c => c.GetAccountId()).Returns(1);

        //Act
        var result = await _walletService.GetWalletsByAccount();

        //Assert
        Assert.Empty(result);
    }

    [Trait("Service", "Wallet")]
    [Fact(DisplayName = "Should add asset")]
    public async Task AddAsset()
    {
        //Arrange
        var wallet = WalletMock.ValidWalletFaker.Generate();
        var addAssetDto = AddAssetInputDtoMock.ValidAddAssetInputRecordFaker.Generate();
        
        _mocker.GetMock<IWalletRepository>().Setup(c => c.GetWalletById(It.IsAny<Guid>())).ReturnsAsync(wallet);
        _mocker.GetMock<IWalletRepository>().Setup(c => c.Update(It.IsAny<Wallet>()));
        
        //Act
        var result = await _walletService.AddAsset(addAssetDto);

        //Assert
        Assert.NotNull(result);
        _mocker.GetMock<IWalletRepository>().Verify(v => v.Update(It.IsAny<Wallet>()), Times.Once);
    }
}