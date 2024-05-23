using Microsoft.AspNetCore.Mvc.Testing;

namespace Teapot.Web.Tests.IntegrationTests;

public class CustomHeaderTests
{

    [Test]
    public async Task CanSetCustomHeaders()
    {
        HttpClient httpClient = new WebApplicationFactory<Program>().CreateDefaultClient();
        string uri = "/200";
        string headerName = "Foo";
        string headerValue = "bar";

        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        request.Headers.Add($"{StatusExtensions.CUSTOM_RESPONSE_HEADER_PREFIX}{headerName}", headerValue);

        using HttpResponseMessage response = await httpClient.SendAsync(request);

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
}
