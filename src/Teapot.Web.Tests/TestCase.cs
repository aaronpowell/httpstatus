using System.Text.Json.Serialization;
using Teapot.Web.Models;

namespace Teapot.Web.Tests;

public class TestCase(int code, string description, string? body, TeapotStatusCodeMetadata teapotStatusCodeMetadata)
{
    [JsonPropertyName("code")]
    public int Code => code;

    [JsonPropertyName("description")]
    public string Description => description;

    [JsonIgnore]
    public string? Body => body ?? $"{Code} {Description}";

    [JsonIgnore]
    public TeapotStatusCodeMetadata TeapotStatusCodeMetadata { get; } = teapotStatusCodeMetadata;

    public override bool Equals(object? obj) => obj is TestCase code && Code == code.Code;

    public override int GetHashCode() => Code.GetHashCode();

    public override string ToString() => $"{Code} {Description}";
}