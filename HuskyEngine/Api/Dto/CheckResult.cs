using System.Text.Json.Serialization;

namespace HuskyEngine.Api.Dto;

public class CheckResult
{
    [JsonPropertyName("errors")] public List<Item> Errors { get; set; }

    public class Item
    {
        [JsonPropertyName("line")] public int Line { get; set; }
        [JsonPropertyName("pos")] public int Position { get; set; }
        [JsonPropertyName("err")] public string Error { get; set; }
    }
}