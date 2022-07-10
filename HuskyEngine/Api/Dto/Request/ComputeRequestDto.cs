using System.Text.Json.Serialization;

namespace HuskyEngine.Api.Dto.Request;

public class ComputeRequestDto
{
    [JsonPropertyName("date")] public DateTime Date { get; set; }
    [JsonPropertyName("formula")] public string Formula { get; set; }
    [JsonPropertyName("strict_type")] public bool StrictType { get; set; } = true;
}