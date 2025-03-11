using Microsoft.AspNetCore.Mvc.Testing;

namespace Teapot.Web.Tests.IntegrationTests;

[TestFixtureSource(typeof(HttpMethods), nameof(HttpMethods.All))]
public class StatusCodeTests(HttpMethod httpMethod)
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

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesWithContent))]
    public async Task ResponseWithContent([Values] TestCase testCase)
    {
        string uri = $"/{testCase.Code}";
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
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
        string uri = $"/{testCase.Code}?{nameof(ResponseOptions.SuppressBody)}=true";
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
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
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
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
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
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

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesWithHeaders))]
    public async Task ResponseWithHeaders([Values] TestCase testCase)
    {
        string uri = $"/{testCase.Code}";
        using HttpRequestMessage httpRequest = new(httpMethod, uri);
        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        Assert.That((int)response.StatusCode, Is.EqualTo(testCase.Code));
        string body = await response.Content.ReadAsStringAsync();
        Assert.Multiple(() =>
        {
            // Some headers as skipped as their info is represented elsewhere in the API, so we'll filter them out
            List<string> headersToSkip = ["Content-Type", "Content-Range"];
            foreach (KeyValuePair<string, string> header in testCase.TeapotStatusCodeMetadata.IncludeHeaders.Where(h => ! headersToSkip.Contains(h.Key)))
            {
                Assert.That(response.Headers.Contains(header.Key), Is.True);
                Assert.That(response.Headers.GetValues(header.Key).FirstOrDefault(), Is.EqualTo(header.Value));
            }
        });
    }
}
