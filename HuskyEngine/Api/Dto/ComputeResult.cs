﻿using System.Text.Json.Serialization;

namespace HuskyEngine.Api.Dto;

public class ComputeResult
{
    [JsonPropertyName("date")] public string Date { get; set; }
    [JsonPropertyName("formula")] public string Formula { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("value")] public object Value { get; set; }
}