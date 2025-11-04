using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SignalRCosmos.Functions;

public class Functions
{
    private static readonly HttpClient HttpClient = new();
    private static string Etag = string.Empty;
    private static int StarCount = 0;
    private readonly ILogger<Functions> _logger;

    public Functions(ILogger<Functions> logger)
    {
        _logger = logger;
    }

    [Function("index")]
    public HttpResponseData GetHomePage([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var indexPath = Path.Combine(projectRoot, "content", "index.html");
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

    //[Function("broadcast")]
    //[SignalROutput(HubName = "serverless")]
    //public static Task<SignalRMessageAction> Broadcast([TimerTrigger("*/5 * * * * *")] TimerInfo timerInfo)
    //{
    //    var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/azure/azure-signalr");
    //    request.Headers.UserAgent.ParseAdd("Serverless");
    //    request.Headers.Add("If-None-Match", Etag);
    //    var response = HttpClient.SendAsync(request).Result;
    //    if (response.Headers.Contains("Etag"))
    //    {
    //        Etag = response.Headers.GetValues("Etag").First();
    //    }
    //    if (response.StatusCode == HttpStatusCode.OK)
    //    {
    //        var result = response.Content.ReadFromJsonAsync<GitResult>().Result;
    //        if (result != null)
    //        {
    //            StarCount = result.StarCount;
    //        }
    //    }
    //    return new SignalRMessageAction("newMessage", new object[] { $"Current star count of https://github.com/Azure/azure-signalr is: {StarCount}" });
    //}

    private class GitResult
    {
        [JsonPropertyName("stargazers_count")]
        public int StarCount { get; set; }
    }
}