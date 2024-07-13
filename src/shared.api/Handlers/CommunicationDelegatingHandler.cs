using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace shared.api.Handlers;

public class CommunicationDelegatingHandler : DelegatingHandler
{
    public readonly ILogger<CommunicationDelegatingHandler> _logger;

    public CommunicationDelegatingHandler(ILogger<CommunicationDelegatingHandler> logger)
    {
        _logger=logger;
    }


    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request path: {path}", request.RequestUri);

        if (request.Content is not null)        
            _logger.LogInformation("Request: {content}", JsonConvert.SerializeObject(await request.Content.ReadAsStringAsync(cancellationToken)));
        
        var response = await base.SendAsync(request, cancellationToken);

        if(response.Content is not null)
            _logger.LogInformation("Response: {content}", JsonConvert.SerializeObject(await response.Content.ReadAsStringAsync(cancellationToken)));

        return response;
    }
}