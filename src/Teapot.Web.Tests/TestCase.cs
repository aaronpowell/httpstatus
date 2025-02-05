using System.Text.Json.Serialization;
using Teapot.Web.Models;

namespace Teapot.Web.Tests;

public class TestCase
{
    public TestCase(int code, string description, string? body, TeapotStatusCodeMetadata teapotStatusCodeMetadata)
    {
        Code = code;
        Description = description;
        Body = body ?? $"{Code} {Description}";
        TeapotStatusCodeMetadata = teapotStatusCodeMetadata;
    }

    [JsonPropertyName("code")]
    public int Code { get; }

    [JsonPropertyName("description")]
    public string Description { get; }

    [JsonIgnore]
    public string? Body { get; }

    [JsonIgnore]
    public TeapotStatusCodeMetadata TeapotStatusCodeMetadata { get; }

    public override bool Equals(object? obj) => obj is TestCase code && Code == code.Code;

    public override int GetHashCode() => Code.GetHashCode();

    public override string ToString() => $"{Code} {Description}";
}