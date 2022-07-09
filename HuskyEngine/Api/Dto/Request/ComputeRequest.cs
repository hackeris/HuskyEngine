using System.Text.Json.Serialization;

namespace HuskyEngine.Api.Dto.Request;

public class ComputeRequest
{
    [JsonPropertyName("date")] public DateTime Date { get; set; }
    [JsonPropertyName("formula")] public string Formula { get; set; }
    [JsonPropertyName("bool_as_number")] public bool BoolAsNumber { get; set; } = true;
}