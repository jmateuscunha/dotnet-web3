using shared.api.Dtos.Inputs;
using shared.api.Dtos.Ouputs;

namespace service.api.Interfaces;

public interface IAccountService
{
    Task Add(AccountInputDto account);
    Task<AccountOutputDto> GetByEmail(string email);
    Task<VerifyLoginOutputDto> Verify(string email, string password);
    Task<VerifyLoginOutputDto> VerifyRefreshAndRenew(string refreshToken);
}