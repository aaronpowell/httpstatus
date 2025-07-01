using System.ComponentModel.DataAnnotations;

namespace Teapot.Web.Configuration;

public class TimeoutOptions
{
    public const string SectionName = "Timeout";

    /// <summary>
    /// Maximum sleep timeout in milliseconds. Default is 30 seconds (30,000 ms).
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "MaxSleepMilliseconds must be a non-negative value")]
    public int MaxSleepMilliseconds { get; set; } = 30 * 1000; // 30 seconds default
}