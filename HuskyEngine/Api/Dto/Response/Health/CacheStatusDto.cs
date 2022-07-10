using System.Text.Json.Serialization;

namespace HuskyEngine.Api.Dto.Response.Health;

public class CacheStatusDto
{
    [JsonPropertyName("maxSize")]
    public int MaxSize { get; set; }
    [JsonPropertyName("used")]
    public int Used { get; set; }
}