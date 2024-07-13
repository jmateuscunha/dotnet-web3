using Microsoft.IdentityModel.Tokens;
using model.api;
using repository.api.Interfaces;
using service.api.Interfaces;
using shared.api.Dtos;
using shared.api.Dtos.Inputs;
using shared.api.Dtos.Ouputs;
using shared.api.Enums;
using shared.api.Validations.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace service.api;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IDomainValidation _validation;
    private readonly ApiConfiguration _configuration;
    public AccountService(IAccountRepository accountRepository,
                          IRoleRepository roleRepository,
                          IDomainValidation validation,
                          ApiConfiguration configuration)
    {
        _accountRepository=accountRepository;
        _roleRepository=roleRepository;
        _validation=validation;
        _configuration=configuration;
    }

    public async Task Add(AccountInputDto dto)
    {
        var role = await _roleRepository.GetRoleByName(dto.Role);

        if (role is null)
        {
            _validation.AddError("Role not found.");
            return;
        }

        var account = await _accountRepository.GetByEmail(dto.Email);

        if (account is not null)
        {
            _validation.AddError("Email already in use.");
            return;
        }

        var pwdHashed = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password,16);

        var item = new Account(dto.Email, pwdHashed);
        item.Roles.Add(role);

        await _accountRepository.Add(item);
    }

    public async Task<AccountOutputDto> GetByEmail(string email)
    {
        var account = await _accountRepository.GetByEmail(email);

        if (account is null)
        {
            _validation.AddError("Invalid credentials.");
            return null;
        }

        return new AccountOutputDto { Id=account.Id, Email = email, Password = account.Password };
    }

    public async Task<VerifyLoginOutputDto> Verify(string email, string password)
    {
        var account = await _accountRepository.GetByEmail(email);

        var isVerified = BCrypt.Net.BCrypt.EnhancedVerify(password, account.Password);

        if (!isVerified)
        {
            _validation.AddError("Invalid Credentials.");
            return null;
        }

        var accessToken = GenerateToken(account.Id.ToString(), JwtTokenType.AccessToken);
        var refreshToken = GenerateToken(account.Id.ToString(), JwtTokenType.RefreshToken);

        return new VerifyLoginOutputDto { AccessToken = accessToken, RefreshToken = refreshToken, Expires = 3600 };
    }

    public async Task<VerifyLoginOutputDto> VerifyRefreshAndRenew(string refreshToken)
    {
        var validationParameters = new TokenValidationParameters()
        {
            ValidIssuer = _configuration.ApiIssuer,
            ValidAudience = _configuration.ApiAudience,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ClockSkew = TimeSpan.FromMinutes(5),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(_configuration.ApiIssuerKey))
        };

        var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validationParameters, out _);
        var accountId = claimsPrincipal.FindFirst("accountId");

        var accessTokenRenewed = GenerateToken(accountId.Value, JwtTokenType.AccessToken);
        var refreshTokenRenewed = GenerateToken(accountId.Value, JwtTokenType.RefreshToken);

        return await Task.FromResult(new VerifyLoginOutputDto { AccessToken = accessTokenRenewed, RefreshToken = refreshTokenRenewed, Expires = 3600 });
    }

    private string GenerateToken(string accountId, JwtTokenType type)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateJwtSecurityToken(issuer: _configuration.ApiIssuer,
            audience: _configuration.ApiAudience,
            subject: new ClaimsIdentity(new List<Claim> {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new(JwtRegisteredClaimNames.UniqueName, "sample.api-v0"),
                new("accountId", accountId)
            }),
            notBefore: DateTime.UtcNow,
            expires:  type == JwtTokenType.AccessToken ? DateTime.UtcNow.AddSeconds(3600) : DateTime.UtcNow.AddDays(1),
            signingCredentials:
            new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.Default.GetBytes(_configuration.ApiIssuerKey)),
                    SecurityAlgorithms.HmacSha512Signature));

        return tokenHandler.WriteToken(token);
    }
}