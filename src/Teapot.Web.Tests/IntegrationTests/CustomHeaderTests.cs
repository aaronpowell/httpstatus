using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace Teapot.Web.Tests.IntegrationTests;

public class CustomHeaderTests
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
    public async Task CanSetCustomHeaders()
    {
        string uri = "/200";
        string headerName = "Foo";
        string headerValue = "bar";

        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        request.Headers.Add($"{StatusExtensions.CUSTOM_RESPONSE_HEADER_PREFIX}{headerName}", headerValue);

        using HttpResponseMessage response = await _httpClient.SendAsync(request);

        System.Net.Http.Headers.HttpResponseHeaders headers = response.Headers;
        Assert.Multiple(() =>
        {
            Assert.That(headers.Contains(headerName), Is.True);
            Assert.That(headers.TryGetValues(headerName, out IEnumerable<string>? values), Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values!.Count(), Is.EqualTo(1));
            Assert.That(values!.First(), Is.EqualTo(headerValue));
        });
    }

    [Test]
    public async Task CanSetCustomHeadersCaseInsensitive()
    {
        string uri = "/200";
        string headerName = "Foo";
        string headerValue = "bar";

        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        request.Headers.Add($"{StatusExtensions.CUSTOM_RESPONSE_HEADER_PREFIX.ToLower()}{headerName}", headerValue);

        using HttpResponseMessage response = await _httpClient.SendAsync(request);

        System.Net.Http.Headers.HttpResponseHeaders headers = response.Headers;
        Assert.Multiple(() =>
        {
            Assert.That(headers.Contains(headerName), Is.True);
            Assert.That(headers.TryGetValues(headerName, out IEnumerable<string>? values), Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values!.Count(), Is.EqualTo(1));
            Assert.That(values!.First(), Is.EqualTo(headerValue));
        });
    }

    [Test]
    public async Task Redirects302ToCorrectLocation()
    {
        string uri = "/302";
        string headerName = "Location";
        string headerValue = "example.com";

        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        request.Headers.Add($"{StatusExtensions.CUSTOM_RESPONSE_HEADER_PREFIX}{headerName}", headerValue);

        using var response = await _httpClient.SendAsync(request);

        var headers = response.Headers;
        Assert.Multiple(() =>
        {
            Assert.That(headers.Contains(headerName), Is.True);
            Assert.That(headers.TryGetValues(headerName, out var values), Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values!.Count(), Is.EqualTo(1));
            Assert.That(values!.First(), Is.EqualTo(headerValue));
        });
    }
}
