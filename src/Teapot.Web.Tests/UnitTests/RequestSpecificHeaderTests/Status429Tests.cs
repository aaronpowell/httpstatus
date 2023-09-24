using System.Globalization;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;

using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

namespace Teapot.Web.Tests.UnitTests.RequestSpecificHeaderTests;
internal class Status429Tests
{
    private TeapotStatusCodeResults _statusCodes;
    private TeapotStatusCodeResult _tooManyRequestsCodeResult { get => _statusCodes[429]; }

    private string _dateFormat = @"ddd, dd MMM yyyy HH:mm:ss \G\M\T";
    private CultureInfo _enGbCulture = CultureInfo.GetCultureInfo("en-GB");

    [SetUp]
    public void Setup()
    {
        _statusCodes = new(
            new AmazonStatusCodeResults(),
            new CloudflareStatusCodeResults(),
            new EsriStatusCodeResults(),
            new LaravelStatusCodeResults(),
            new MicrosoftStatusCodeResults(),
            new NginxStatusCodeResults(),
            new TwitterStatusCodeResults()
        );
    }

    [Test]
    public void GetRequestSpecificHeadersDefined()
    {
        Assert.IsNotNull(_tooManyRequestsCodeResult.GetRequestSpecificHeaders);
    }

    [Test]
    public void DefaultHeaders()
    {
        var queryCollection = new QueryCollection();
        var headers = new HeaderDictionary();

        var expected = new Dictionary<string, string>()
        {
            { "Retry-After", "5" }
        };

        Assert.IsNotNull(_tooManyRequestsCodeResult.GetRequestSpecificHeaders);
        var actual = _tooManyRequestsCodeResult.GetRequestSpecificHeaders(queryCollection, headers);

        foreach (var kvp in expected)
        {
            Assert.That(actual, Contains.Key(kvp.Key));
            Assert.That(actual[kvp.Key], Is.EqualTo(kvp.Value));
        }
    }

    [Test]
    [TestCase("", "5")]
    [TestCase("0", "0")]
    [TestCase("5", "5")]
    [TestCase("10", "10")]
    [TestCase("100000", "100000")]
    public void SecondsQuery(string secondsQuery, string secondsExpected)
    {
        var queryCollection = new QueryCollection(new Dictionary<string, StringValues>()
        {
            {"seconds", new StringValues(secondsQuery) }
        });
        var headers = new HeaderDictionary();

        var expected = new Dictionary<string, string>()
        {
            { "Retry-After", secondsExpected }
        };

        Assert.IsNotNull(_tooManyRequestsCodeResult.GetRequestSpecificHeaders);
        var actual = _tooManyRequestsCodeResult.GetRequestSpecificHeaders(queryCollection, headers);

        foreach (var kvp in expected)
        {
            Assert.That(actual, Contains.Key(kvp.Key));
            Assert.That(actual[kvp.Key], Is.EqualTo(kvp.Value));
        }
    }

    [Test]
    [TestCase("", "5")]
    [TestCase("0", "0")]
    [TestCase("5", "5")]
    [TestCase("10", "10")]
    [TestCase("100000", "100000")]
    public void SecondsHeader(string secondsHeader, string secondsExpected)
    {
        var queryCollection = new QueryCollection();
        var headers = new HeaderDictionary(new Dictionary<string, StringValues>()
        {
            { "X-Retry-After-Seconds", secondsHeader }
        });

        var expected = new Dictionary<string, string>()
        {
            { "Retry-After", secondsExpected }
        };

        Assert.IsNotNull(_tooManyRequestsCodeResult.GetRequestSpecificHeaders);
        var actual = _tooManyRequestsCodeResult.GetRequestSpecificHeaders(queryCollection, headers);

        foreach (var kvp in expected)
        {
            Assert.That(actual, Contains.Key(kvp.Key));
            Assert.That(actual[kvp.Key], Is.EqualTo(kvp.Value));
        }
    }

    [Test]
    public void SecondsFormatQuery()
    {
        var queryCollection = new QueryCollection(new Dictionary<string, StringValues>()
        {
            {"format", new StringValues("seconds") }
        });
        var headers = new HeaderDictionary();

        var expected = new Dictionary<string, string>()
        {
            { "Retry-After", "5" }
        };

        Assert.IsNotNull(_tooManyRequestsCodeResult.GetRequestSpecificHeaders);
        var actual = _tooManyRequestsCodeResult.GetRequestSpecificHeaders(queryCollection, headers);

        foreach (var kvp in expected)
        {
            Assert.That(actual, Contains.Key(kvp.Key));
            Assert.That(actual[kvp.Key], Is.EqualTo(kvp.Value));
        }
    }

    [Test]
    public void SecondsFormatHeader()
    {
        var queryCollection = new QueryCollection();
        var headers = new HeaderDictionary(new Dictionary<string, StringValues>()
        {
            {"X-Retry-After-Format", new StringValues("seconds") }
        });

        var expected = new Dictionary<string, string>()
        {
            { "Retry-After", "5" }
        };

        Assert.IsNotNull(_tooManyRequestsCodeResult.GetRequestSpecificHeaders);
        var actual = _tooManyRequestsCodeResult.GetRequestSpecificHeaders(queryCollection, headers);

        foreach (var kvp in expected)
        {
            Assert.That(actual, Contains.Key(kvp.Key));
            Assert.That(actual[kvp.Key], Is.EqualTo(kvp.Value));
        }
    }

    [Test]
    public void DateFormatQuery()
    {
        var queryCollection = new QueryCollection(new Dictionary<string, StringValues>()
        {
            {"format", new StringValues("date") }
        });
        var headers = new HeaderDictionary();

        var expectedDate = DateTime.UtcNow.AddSeconds(5);

        Assert.IsNotNull(_tooManyRequestsCodeResult.GetRequestSpecificHeaders);
        var actual = _tooManyRequestsCodeResult.GetRequestSpecificHeaders(queryCollection, headers);

        Assert.That(actual, Contains.Key("Retry-After"));

        var actualValue = actual["Retry-After"];
        var actualDate = DateTime.ParseExact(actualValue, _dateFormat, _enGbCulture);
        Assert.That(actualDate, Is.EqualTo(expectedDate).Within(1).Seconds);
    }

    [Test]
    public void DateFormatHeader()
    {
        var queryCollection = new QueryCollection();
        var headers = new HeaderDictionary(new Dictionary<string, StringValues>()
        {
            {"X-Retry-After-Format", new StringValues("date") }
        });

        var expectedDate = DateTime.UtcNow.AddSeconds(5);

        Assert.IsNotNull(_tooManyRequestsCodeResult.GetRequestSpecificHeaders);
        var actual = _tooManyRequestsCodeResult.GetRequestSpecificHeaders(queryCollection, headers);

        Assert.That(actual, Contains.Key("Retry-After"));

        var actualValue = actual["Retry-After"];
        var actualDate = DateTime.ParseExact(actualValue, _dateFormat, _enGbCulture);
        Assert.That(actualDate, Is.EqualTo(expectedDate).Within(1).Seconds);
    }
}
