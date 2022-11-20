using System.Text.Json.Serialization;

namespace Teapot.Web.Tests;

public readonly struct ExtendedHttpStatusCode
{
    public ExtendedHttpStatusCode(int code, string description)
    {
        Code = code;
        Description = description;
    }

    [JsonPropertyName("code")]
    public int Code { get; }

    [JsonPropertyName("description")]
    public string Description { get; }

    public override bool Equals(object? obj) => obj is ExtendedHttpStatusCode code && Code == code.Code;

    public override int GetHashCode() => Code.GetHashCode();

    public override string ToString() => $"{Code} {Description}";
}