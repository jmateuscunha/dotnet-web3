using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using shared.api.Dtos;
using shared.api.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;

namespace shared.api.Handlers;

public class SampleAuthenticationOptions : AuthenticationSchemeOptions
{
}

public class AuthenticationHandler : AuthenticationHandler<SampleAuthenticationOptions>
{
    private readonly ApiConfiguration _configuration;
    public AuthenticationHandler(IOptionsMonitor<SampleAuthenticationOptions> authenticationOptions,
                                       ILoggerFactory logger,
                                       UrlEncoder encoder,
                                       ApiConfiguration configuration) : base(authenticationOptions, logger, encoder)
    {
        _configuration=configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            return await Task.FromResult(AuthenticateResult.NoResult());

        var bearerToken = Request.Headers["Authorization"].FirstOrDefault() ?? throw new AuthenticationException("SAMPLE-010","Authorization token is missing or invalid.");

        bearerToken = bearerToken.Replace("Bearer ", "", StringComparison.InvariantCultureIgnoreCase);

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

        var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(bearerToken, validationParameters, out _);

        if (Context.Response.StatusCode < 400 && claimsPrincipal != null)
            Context.User = claimsPrincipal;

        return AuthenticateResult.Success
               (new AuthenticationTicket(claimsPrincipal,
               Scheme.Name));

    }
}