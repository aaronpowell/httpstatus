using Microsoft.AspNetCore.Mvc.Testing;

namespace Teapot.Web.Tests.IntegrationTests;

[TestFixtureSource(typeof(HttpMethods), nameof(HttpMethods.All))]
public class RandomTests(HttpMethod httpMethod)
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _httpClient = new WebApplicationFactory<Program>().CreateDefaultClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _httpClient.Dispose();
    }

    private HttpClient _httpClient = null!;

    [TestCase("foo")]
    [TestCase("200,x")]
    [TestCase("200.0")]
    [TestCase("-1,0,1")]
    [TestCase("-1-1")]
    public async Task ParseError(string input)
    {
        string uri = $"/random/{input}";
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase("190,290,390")]
    [TestCase("190-193")]
    [TestCase("190,192,193,194-196")]
    [TestCase("190,290,190,290")]
    [TestCase("190-199,570-590")]
    public async Task ParsedAsExpected(string input)
    {
        string uri = $"/random/{input}";
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.InRange(100, 599));
        string body = await response.Content.ReadAsStringAsync();
        Assert.Multiple(() =>
        {
            Assert.That(body.ReplaceLineEndings(), Does.EndWith("Unknown Code"));
            Assert.That(response.Content?.Headers?.ContentType, Is.Not.Null);
            Assert.That(response.Content?.Headers?.ContentType?.MediaType, Is.EqualTo("text/plain"));
            Assert.That(response.Content?.Headers?.ContentLength, Is.EqualTo(body.Length));
        });
    }
}
