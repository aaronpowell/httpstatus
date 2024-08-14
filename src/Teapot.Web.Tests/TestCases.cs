using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;
using static System.Net.HttpStatusCode;

namespace Teapot.Web.Tests;

public class TestCases
{
    private static readonly TeapotStatusCodeMetadataCollection All = new(
            new AmazonStatusCodeMetadata(),
            new CloudflareStatusCodeMetadata(),
            new EsriStatusCodeMetadata(),
            new LaravelStatusCodeMetadata(),
            new MicrosoftStatusCodeMetadata(),
            new NginxStatusCodeMetadata(),
            new TwitterStatusCodeMetadata()
            );

    private static readonly HttpStatusCode[] NoContentStatusCodes =
    [
        Continue, SwitchingProtocols, Processing, EarlyHints, NoContent, ResetContent, NotModified
    ];

    public static IEnumerable<TestCase> StatusCodesAll =>
        All.Select(Map);

    public static IEnumerable<TestCase> StatusCodesWithContent =>
        All
        .Where(x => !x.Value.ExcludeBody)
        .Select(Map);

    public static IEnumerable<TestCase> StatusCodesNoContent =>
        NoContentStatusCodes.Select(Map);

    private static TestCase Map(HttpStatusCode code)
    {
        int key = (int)code;
        return new(key, All[key].Description, All[key].Body);
    }

    private static TestCase Map(KeyValuePair<int, TeapotStatusCodeMetadata> code)
    {
        return new(code.Key, code.Value.Description, code.Value.Body);
    }
}
