using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Teapot.Web.Models;

namespace Teapot.Web.Tests.UnitTests;

public class CustomHttpStatusCodeResultTests
{
    private HttpContext _httpContext;
    private HttpResponseFeature _httpResponseFeature;
    private Stream _body;
    private Mock<ActionContext> _mockActionContext;

    [SetUp]
    public void Setup()
    {
        var logger = new Mock<ILogger>();
        var loggerFactory = new Mock<ILoggerFactory>();
        var serviceProvider = new Mock<IServiceProvider>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
        serviceProvider.Setup(x => x.GetService(typeof(ILogger))).Returns(logger.Object);
        serviceProvider.Setup(x => x.GetService(typeof(ILoggerFactory))).Returns(loggerFactory.Object);

        var featureCollection = new FeatureCollection();
        _httpResponseFeature = new HttpResponseFeature();
        _body = new MemoryStream();
        featureCollection.Set<IHttpRequestFeature>(new HttpRequestFeature());
        featureCollection.Set<IHttpResponseFeature>(_httpResponseFeature);
        featureCollection.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(_body));

        _httpContext = new DefaultHttpContext(featureCollection)
        {
            RequestServices = serviceProvider.Object
        };

        var routeData = new Mock<RouteData>();
        var actionDescriptor = new Mock<ActionDescriptor>();
        _mockActionContext = new Mock<ActionContext>(_httpContext, routeData.Object, actionDescriptor.Object);
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesAll))]
    public async Task Response_Is_Correct(TestCaseCodes httpStatusCode)
    {
        var statusCodeResult = new TeapotStatusCodeResult
        {
            Description = httpStatusCode.Description
        };

        var target = new CustomHttpStatusCodeResult(httpStatusCode.Code, statusCodeResult);

        await target.ExecuteResultAsync(_mockActionContext.Object);

        Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(httpStatusCode.Code));
        Assert.That(_httpContext.Response.ContentType, Is.EqualTo("text/plain"));
        Assert.That(_httpResponseFeature.ReasonPhrase, Is.EqualTo(httpStatusCode.Description));

        _body.Position = 0;
        var sr = new StreamReader(_body);
        var body = sr.ReadToEnd();
        Assert.That(body, Is.EqualTo(httpStatusCode.ToString()));
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesAll))]
    public async Task Response_Json_Is_Correct(TestCaseCodes httpStatusCode)
    {
        var statusCodeResult = new TeapotStatusCodeResult
        {
            Description = httpStatusCode.Description
        };

        var target = new CustomHttpStatusCodeResult(httpStatusCode.Code, statusCodeResult);

        _httpContext.Request.Headers.Accept = "application/json";

        await target.ExecuteResultAsync(_mockActionContext.Object);

        Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(httpStatusCode.Code));
        Assert.That(_httpContext.Response.ContentType, Is.EqualTo("application/json"));
        Assert.That(_httpResponseFeature.ReasonPhrase, Is.EqualTo(httpStatusCode.Description));

        _body.Position = 0;
        var sr = new StreamReader(_body);
        var body = sr.ReadToEnd();
        var expectedBody = JsonSerializer.Serialize(httpStatusCode);
        Assert.That(body, Is.EqualTo(expectedBody));
    }
}