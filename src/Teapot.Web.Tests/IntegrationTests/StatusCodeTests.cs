using Microsoft.AspNetCore.Mvc.Testing;

namespace Teapot.Web.Tests.IntegrationTests;

[TestFixtureSource(typeof(HttpMethods), nameof(HttpMethods.All))]
public class StatusCodeTests
{
    private readonly HttpMethod _httpMethod;

    private static readonly HttpClient _httpClient = new WebApplicationFactory<Program>().CreateDefaultClient();

    public StatusCodeTests(HttpMethod httpMethod)
    {
        _httpMethod = httpMethod;
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesWithContent))]
    public async Task ResponseWithContent([Values] TestCase testCase)
    {
        string uri = $"/{testCase.Code}";
        using HttpRequestMessage httpRequest = new HttpRequestMessage(_httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.EqualTo(testCase.Code));
        string body = await response.Content.ReadAsStringAsync();
        Assert.Multiple(() =>
        {
            Assert.That(body.ReplaceLineEndings(), Is.EqualTo(testCase.Body));
            Assert.That(response.Content?.Headers?.ContentType, Is.Not.Null);
            Assert.That(response.Content?.Headers?.ContentType?.MediaType, Is.EqualTo("text/plain"));
            Assert.That(response.Content?.Headers?.ContentLength, Is.EqualTo(body.Length));
        });
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesWithContent))]
    public async Task ResponseWithContentSuppressedViaQs([Values] TestCase testCase)
    {
        string uri = $"/{testCase.Code}?{nameof(CustomHttpStatusCodeResult.SuppressBody)}=true";
        using HttpRequestMessage httpRequest = new HttpRequestMessage(_httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.EqualTo(testCase.Code));
        string body = await response.Content.ReadAsStringAsync();
        Assert.Multiple(() =>
        {
            Assert.That(body, Is.Empty);
            Assert.That(response.Content?.Headers?.ContentType, Is.Null);
            Assert.That(response.Content?.Headers?.ContentType?.MediaType, Is.Null);
            Assert.That(response.Content?.Headers?.ContentLength, Is.EqualTo(0));
        });
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesWithContent))]
    public async Task ResponseWithContentSuppressedViaHeader([Values] TestCase testCase)
    {
        string uri = $"/{testCase.Code}";
        using HttpRequestMessage httpRequest = new HttpRequestMessage(_httpMethod, uri);
        httpRequest.Headers.Add(StatusExtensions.SUPPRESS_BODY_HEADER, "true");
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.EqualTo(testCase.Code));
        string body = await response.Content.ReadAsStringAsync();
        Assert.Multiple(() =>
        {
            Assert.That(body, Is.Empty);
            Assert.That(response.Content?.Headers?.ContentType, Is.Null);
            Assert.That(response.Content?.Headers?.ContentType?.MediaType, Is.Null);
            Assert.That(response.Content?.Headers?.ContentLength, Is.EqualTo(0));
        });
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesNoContent))]
    public async Task ResponseNoContent([Values] TestCase testCase)
    {
        string uri = $"/{testCase.Code}";
        using HttpRequestMessage httpRequest = new HttpRequestMessage(_httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.EqualTo(testCase.Code));
        string body = await response.Content.ReadAsStringAsync();
        Assert.Multiple(() =>
        {
            Assert.That(body, Is.Empty);
            Assert.That(response.Content.Headers.ContentType, Is.Null);
            Assert.That(response.Content.Headers.ContentLength, Is.EqualTo(0));
        });
    }
}
