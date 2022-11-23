using System.Text.Json.Serialization;

namespace Teapot.Web.Tests;

public readonly struct ExtendedHttpStatusCode
{
    public ExtendedHttpStatusCode(int code, string description, string? body)
    {
        Code = code;
        Description = description;
        Body = body ?? $"{Code} {Description}";
    }

    [JsonPropertyName("code")]
    public int Code { get; }

    [JsonPropertyName("description")]
    public string Description { get; }

    [JsonIgnore]
    public string? Body { get; }

    public override bool Equals(object? obj) => obj is ExtendedHttpStatusCode code && Code == code.Code;

    public override int GetHashCode() => Code.GetHashCode();

    public override string ToString() => $"{Code} {Description}";
}