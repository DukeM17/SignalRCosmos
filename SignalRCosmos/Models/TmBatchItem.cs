using System.Text.Json.Serialization;

namespace SignalRCosmos.Models
{
    public class TmBatchItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        // If your Cosmos PK is a string, keep this as string; if numeric, change to int.
        [JsonPropertyName("batchId")]
        public string BatchId { get; set; } = default!;

        [JsonPropertyName("version")]
        public int Version { get; set; }

        // The current overall stage (matches the latest update below)
        [JsonPropertyName("stage")]
        public string Stage { get; set; } = default!;

        [JsonPropertyName("statusUpdates")]
        public List<StatusUpdate> StatusUpdates { get; set; } = new();
    }

    public class StatusUpdate
    {
        [JsonPropertyName("stage")]
        public string Stage { get; set; } = default!;

        [JsonPropertyName("status")]
        public string Status { get; set; } = default!; // e.g., "Completed"

        // Stored as ISO-8601 in Cosmos; map to DateTime for .NET
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("additionalInformation")]
        public string AdditionalInformation { get; set; }
    }
}