using model.api;
using Moq;
using Moq.AutoMock;
using repository.api.Interfaces;
using sample.test.api.Fakers;
using service.api;
using service.api.Interfaces;
using shared.api.Validations.Interfaces;

namespace sample.test.api.Services;

public class AccountServiceTest
{
    private readonly AutoMocker _mocker;
    private readonly IAccountService _accountService;
    public AccountServiceTest()
    {
        _mocker = new AutoMocker();
        _accountService = _mocker.CreateInstance<AccountService>();
    }

    [Trait("Service", "Account")]
    [Fact(DisplayName = "Should Add Account")]
    public async Task AddAccount_Valid()
    {
        //Arrange
        var accountDto = AccountDtoMock.ValidAccountDtoFaker.Generate();
        var role = RoleMock.ValidRoleFaker.Generate();
        _mocker.GetMock<IRoleRepository>().Setup(c => c.GetRoleByName(It.IsAny<string>())).ReturnsAsync(role);
        _mocker.GetMock<IAccountRepository>().Setup(c => c.GetByEmail(It.IsAny<string>())).ReturnsAsync(It.IsAny<Account>());
        //Act
        await _accountService.Add(accountDto);

        //Assert
        _mocker.GetMock<IRoleRepository>().Verify(v => v.GetRoleByName(It.IsAny<string>()), Times.Once);
        _mocker.GetMock<IAccountRepository>().Verify(v => v.Add(It.IsAny<Account>()), Times.Once);
    }

    [Trait("Service", "Account")]
    [Fact(DisplayName = "Invalid Role - Should Not Add Account")]
    public async Task AddAccount_Invalid_Null()
    {
        //Arrange
        var accountDto = AccountDtoMock.ValidAccountDtoFaker.Generate();
        _mocker.GetMock<IRoleRepository>().Setup(c => c.GetRoleByName(It.IsAny<string>())).ReturnsAsync(It.IsAny<Role>());
        _mocker.GetMock<IAccountRepository>().Setup(c => c.GetByEmail(It.IsAny<string>())).ReturnsAsync(It.IsAny<Account>());

        //Act
        await _accountService.Add(accountDto);

        //Assert
        _mocker.GetMock<IRoleRepository>().Verify(v => v.GetRoleByName(It.IsAny<string>()), Times.Once);
        _mocker.GetMock<IAccountRepository>().Verify(v => v.Add(It.IsAny<Account>()), Times.Never);
        _mocker.GetMock<IDomainValidation>().Verify(v => v.AddError(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Trait("Service", "Account")]
    [Fact(DisplayName = "Invalid Email - Should Not Add Account")]
    public async Task AddAccount_Email_InUse()
    {
        //Arrange
        var accountDto = AccountDtoMock.ValidAccountDtoFaker.Generate();
        var role = RoleMock.ValidRoleFaker.Generate();
        var account = AccountMock.ValidAccountFaker.Generate();
        _mocker.GetMock<IRoleRepository>().Setup(c => c.GetRoleByName(It.IsAny<string>())).ReturnsAsync(role);
        _mocker.GetMock<IAccountRepository>().Setup(c => c.GetByEmail(It.IsAny<string>())).ReturnsAsync(account);

        //Act
        await _accountService.Add(accountDto);

        //Assert
        _mocker.GetMock<IRoleRepository>().Verify(v => v.GetRoleByName(It.IsAny<string>()), Times.Once);
        _mocker.GetMock<IAccountRepository>().Verify(v => v.Add(It.IsAny<Account>()), Times.Never);
        _mocker.GetMock<IDomainValidation>().Verify(v => v.AddError(It.IsAny<string>()), Times.AtLeastOnce);
    }
}