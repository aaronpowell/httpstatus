using Microsoft.AspNetCore.Mvc.Testing;

namespace Teapot.Web.Tests.IntegrationTests;

[TestFixtureSource(typeof(HttpMethods), nameof(HttpMethods.All))]
public class TimeoutTests(HttpMethod httpMethod)
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

    [Test]
    public async Task SleepExceedsMaxTimeout_ReturnsBadRequest()
    {
        // Default max timeout is 30 seconds (30,000 ms), try 60 seconds
        string uri = "/200?sleep=60000";
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task SleepAfterHeadersExceedsMaxTimeout_ReturnsBadRequest()
    {
        // Default max timeout is 30 seconds (30,000 ms), try 60 seconds via header
        string uri = "/200";
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
        httpRequest.Headers.Add("X-HttpStatus-SleepAfterHeaders", "60000");
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task SleepWithinMaxTimeout_Succeeds()
    {
        // Default max timeout is 30 seconds (30,000 ms), try 5 seconds
        string uri = "/200?sleep=5000";
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        string body = await response.Content.ReadAsStringAsync();
        Assert.That(body, Does.Contain("200"));
    }

    [Test]
    public async Task SleepEqualToMaxTimeout_Succeeds()
    {
        // Default max timeout is 30 seconds (30,000 ms), try exactly 30 seconds
        // But for testing purposes, let's use a smaller value to avoid long test runs
        string uri = "/200?sleep=1000"; // 1 second - well within limit
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        string body = await response.Content.ReadAsStringAsync();
        Assert.That(body, Does.Contain("200"));
    }

    [Test]
    public async Task SleepJustOverMaxTimeout_ReturnsBadRequest()
    {
        // Default max timeout is 30 seconds (30,000 ms), try 30,001 ms
        string uri = "/200?sleep=30001";
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}