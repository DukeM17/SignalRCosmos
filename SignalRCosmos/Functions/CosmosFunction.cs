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

    [Function("BatchCosmosFunction")]
    [SignalROutput(HubName = "serverless")]
    public Task<SignalRMessageAction> Run([CosmosDBTrigger(
        databaseName: "Batch",
        containerName: "BatchRun",
        Connection = "CosmosDbConnectionString",
        LeaseContainerName = "batch-leases",
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<TmBatchItem> input)
    {
        if (input != null && input.Count > 0)
        {
            foreach(var item in input)
            {
                _logger.LogInformation("Batch Item modified: " + item.Id);
                return Task.FromResult(new SignalRMessageAction("batches", new object[] { item }));
            }
        }
        return Task.FromResult(new SignalRMessageAction("batches", []));
    }
}