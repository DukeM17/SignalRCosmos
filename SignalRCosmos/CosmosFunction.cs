using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalRCosmos.Models;

namespace SignalRCosmos;

public class CosmosFunction
{
    private readonly ILogger<CosmosFunction> _logger;

    public CosmosFunction(ILogger<CosmosFunction> logger)
    {
        _logger = logger;
    }

    [Function("OrderCosmosFunction")]
    [SignalROutput(HubName = "serverless")]
    public async Task<SignalRMessageAction> Run([CosmosDBTrigger(
        databaseName: "OrderDB",
        containerName: "Order",
        Connection = "CosmosConnectionSetting",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Order> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified: " + input.Count);
            _logger.LogInformation("First document Id: " + input[0].Id);
        }
        return new SignalRMessageAction("newMessage", new object[] { "YES" });
    }

    [Function("BatchCosmosFunction")]
    [SignalROutput(HubName = "serverless")]
    public async Task<SignalRMessageAction> RunAsync([CosmosDBTrigger(
        databaseName: "Batch",
        containerName: "BatchRun",
        Connection = "CosmosConnectionSetting",
        LeaseContainerName = "batch-leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<TmBatchItem> input)
    {
        if (input != null && input.Count > 0)
        {
            foreach(var item in input)
            {
                _logger.LogInformation("Batch Item modified: " + item.Id);
                return new SignalRMessageAction("batches", new object[] { item });
            }
        }
        return new SignalRMessageAction("batches", []);
    }
}