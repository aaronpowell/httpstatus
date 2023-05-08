using Microsoft.AspNetCore.Mvc.Testing;
using Teapot.Web.Controllers;

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
        var uri = $"/{testCase.Code}";
        using var httpRequest = new HttpRequestMessage(_httpMethod, uri);
        using var response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.EqualTo(testCase.Code));
        var body = await response.Content.ReadAsStringAsync();
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
        var uri = $"/{testCase.Code}?{nameof(CustomHttpStatusCodeResult.SuppressBody)}=true";
        using var httpRequest = new HttpRequestMessage(_httpMethod, uri);
        using var response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.EqualTo(testCase.Code));
        var body = await response.Content.ReadAsStringAsync();
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
        var uri = $"/{testCase.Code}";
        using var httpRequest = new HttpRequestMessage(_httpMethod, uri);
        httpRequest.Headers.Add(StatusController.SUPPRESS_BODY_HEADER, "true");
        using var response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.EqualTo(testCase.Code));
        var body = await response.Content.ReadAsStringAsync();
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
        var uri = $"/{testCase.Code}";
        using var httpRequest = new HttpRequestMessage(_httpMethod, uri);
        using var response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.EqualTo(testCase.Code));
        var body = await response.Content.ReadAsStringAsync();
        Assert.Multiple(() =>
        {
            Assert.That(body, Is.Empty);
            Assert.That(response.Content.Headers.ContentType, Is.Null);
            Assert.That(response.Content.Headers.ContentLength, Is.EqualTo(0));
        });
    }
}
