using Microsoft.AspNetCore.Mvc.Testing;

namespace Teapot.Web.Tests.IntegrationTests;

public class IntegrationTests
{
    private static readonly HttpClient _httpClient = new WebApplicationFactory<Program>().CreateDefaultClient();

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesWithContent))]
    public async Task ResponseWithContent([Values] TestCaseCodes httpStatusCode)
    {
        var uri = $"/{httpStatusCode.Code}";
        using var response = await _httpClient.GetAsync(uri);
        Assert.That((int)response.StatusCode, Is.EqualTo(httpStatusCode.Code));
        var body = await response.Content.ReadAsStringAsync();
        Assert.That(body.ReplaceLineEndings(), Is.EqualTo(httpStatusCode.Body));
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesNoContent))]
    public async Task ResponseNoContent([Values] TestCaseCodes httpStatusCode)
    {
        var uri = $"/{httpStatusCode.Code}";
        using var response = await _httpClient.GetAsync(uri);
        Assert.That((int)response.StatusCode, Is.EqualTo(httpStatusCode.Code));
        var body = await response.Content.ReadAsStringAsync();
        Assert.That(body, Is.Empty);
    }
}
