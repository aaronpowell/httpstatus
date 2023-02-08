using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Teapot.Web.Tests.IntegrationTests;

[TestFixtureSource(typeof(HttpMethods), nameof(HttpMethods.All))]
public class RandomTests
{
    private readonly HttpMethod _httpMethod;

    private static readonly HttpClient _httpClient = new WebApplicationFactory<Program>().CreateDefaultClient();

    public RandomTests(HttpMethod httpMethod)
    {
        _httpMethod = httpMethod;
    }

    [TestCase("foo")]
    [TestCase("200,x")]
    [TestCase("200.0")]
    [TestCase("-1,0,1")]
    [TestCase("-1-1")]
    public async Task ParseError(string input)
    {
        var uri = $"/random/{input}";
        using var httpRequest = new HttpRequestMessage(_httpMethod, uri);
        using var response = await _httpClient.SendAsync(httpRequest);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase("190,290,390")]
    [TestCase("190-193")]
    [TestCase("190,192,193,194-196")]
    [TestCase("190,290,190,290")]
    [TestCase("190-199,570-590")]
    public async Task ParsedAsExpected(string input)
    {
        var uri = $"/random/{input}";
        using var httpRequest = new HttpRequestMessage(_httpMethod, uri);
        using var response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.InRange(100, 599));
        var body = await response.Content.ReadAsStringAsync();
        Assert.Multiple(() =>
        {
            Assert.That(body.ReplaceLineEndings(), Does.EndWith("Unknown Code"));
            Assert.That(response.Content?.Headers?.ContentType, Is.Not.Null);
            Assert.That(response.Content?.Headers?.ContentType?.MediaType, Is.EqualTo("text/plain"));
            Assert.That(response.Content?.Headers?.ContentLength, Is.EqualTo(body.Length));
        });
    }
}
