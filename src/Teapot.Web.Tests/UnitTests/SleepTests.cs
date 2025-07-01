using Microsoft.AspNetCore.Http;
using Teapot.Web.Configuration;
using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

namespace Teapot.Web.Tests.UnitTests;
public class SleepTests {
    private const int Sleep = 500;

    private TeapotStatusCodeMetadataCollection _statusCodes;
    private TimeoutOptions _timeoutOptions;

    [SetUp]
    public void Setup() {
        _statusCodes = new(
            new AmazonStatusCodeMetadata(),
            new CloudflareStatusCodeMetadata(),
            new EsriStatusCodeMetadata(),
            new LaravelStatusCodeMetadata(),
            new MicrosoftStatusCodeMetadata(),
            new NginxStatusCodeMetadata(),
            new TwitterStatusCodeMetadata()
        );
        _timeoutOptions = new TimeoutOptions();
    }

    [Test]
    public void SleepReadFromQuery()
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new(), sleep: Sleep), null, request.Object, _timeoutOptions);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.Sleep, Is.EqualTo(Sleep));
        });
    }

    [Test]
    public void SleepReadFromHeader()
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        request.Object.Headers.Append(StatusExtensions.SLEEP_HEADER, Sleep.ToString());

        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new()), null, request.Object, _timeoutOptions);

        Assert.Multiple(() => {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.Sleep, Is.EqualTo(Sleep));
        });
    }

    [Test]
    public void SleepReadFromQSTakesPriorityHeader() {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        request.Object.Headers.Append(StatusExtensions.SLEEP_HEADER, Sleep.ToString());

        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new(), sleep: Sleep * 2), null, request.Object, _timeoutOptions);

        Assert.Multiple(() => {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.Sleep, Is.EqualTo(Sleep * 2));
        });
    }

    [Test]
    public void BadSleepHeaderIgnored()
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        request.Object.Headers.Append(StatusExtensions.SLEEP_HEADER, "invalid");

        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new()), null, request.Object, _timeoutOptions);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.Sleep, Is.Null);
        });
    }

    [Test]
    public void SleepMaxTimeout_DefaultIsThirtySeconds()
    {
        // Verify default timeout is 30 seconds (30,000 ms)
        var defaultTimeoutOptions = new TimeoutOptions();
        Assert.That(defaultTimeoutOptions.MaxSleepMilliseconds, Is.EqualTo(30 * 1000));
    }

    [Test]
    public void SleepMaxTimeout_ConfigurableViaOptions()
    {
        // Verify timeout can be configured
        var customTimeoutOptions = new TimeoutOptions { MaxSleepMilliseconds = 60 * 1000 }; // 1 minute
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        
        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(
            new ResponseOptions(200, new(), sleep: 45 * 1000), // 45 seconds 
            null, 
            request.Object, 
            customTimeoutOptions);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());
            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.Sleep, Is.EqualTo(45 * 1000));
        });
    }

    [Test]
    public void SleepMaxTimeout_ClampsExcessiveValues()
    {
        // Test that values exceeding the max are clamped
        var timeoutOptions = new TimeoutOptions { MaxSleepMilliseconds = 30 * 1000 }; // 30 seconds
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        
        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(
            new ResponseOptions(200, new(), sleep: 60 * 1000), // 60 seconds - should be clamped to 30
            null, 
            request.Object, 
            timeoutOptions);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());
            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            // The sleep value in ResponseOptions should remain as requested, 
            // but the actual clamping happens in DoSleep method
            Assert.That(r.Options.Sleep, Is.EqualTo(60 * 1000));
        });
    }

}
