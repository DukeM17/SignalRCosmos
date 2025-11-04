using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
namespace SignalRCosmos.Functions;

public class Functions
{
    private readonly ILogger<Functions> _logger;

    public Functions(ILogger<Functions> logger)
    {
        _logger = logger;
    }

    [Function("index")]
    public HttpResponseData GetHomePage([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        var indexPath = Path.Combine(AppContext.BaseDirectory, "Content", "index.html");
        response.WriteStringAsync(File.ReadAllText(indexPath));
        response.Headers.Add("Content-Type", "text/html");

        return response;
    }

    [Function("negotiate")]
    public HttpResponseData Negotiate([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "options")] HttpRequestData req,
        [SignalRConnectionInfoInput(HubName = "serverless")] string connectionInfo)
    {
        _logger.LogInformation("In negotiate function");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        response.WriteStringAsync(connectionInfo);
        _logger.LogInformation("Connection info: " + connectionInfo);

        return response;
    }
}