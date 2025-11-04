using System.Text.Json.Serialization;

namespace SignalRCosmos.Models
{
    public class TmBatchItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("batchId")]
        public string BatchId { get; set; } = default!;

        [JsonPropertyName("version")]
        public int Version { get; set; }

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
        public string Status { get; set; } = default!;

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("additionalInformation")]
        public string? AdditionalInformation { get; set; }
    }
}