using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

namespace Teapot.Web.Tests.UnitTests;

public class TeapotStatusCodeResultsTests
{
    private TeapotStatusCodeMetadataCollection _target;

    [SetUp]
    public void Setup()
    {
        _target = new TeapotStatusCodeMetadataCollection(
            new AmazonStatusCodeMetadata(),
            new CloudflareStatusCodeMetadata(),
            new EsriStatusCodeMetadata(),
            new LaravelStatusCodeMetadata(),
            new MicrosoftStatusCodeMetadata(),
            new NginxStatusCodeMetadata(),
            new TwitterStatusCodeMetadata()
            );
    }

    [Test]
    public void ContainsStandardCode([Values] HttpStatusCode httpStatusCode)
    {
        Assert.That(_target.ContainsKey((int)httpStatusCode));
    }
}
