using System.Net;

namespace Teapot.LoadTestUI.Models;

public class LoadTestResult
{
    public int TotalRequests { get; set; }
    public int SuccessfulRequests { get; set; }
    public int FailedRequests { get; set; }
    public List<HttpStatusCode> StatusCodes { get; set; } = new();
    public List<long> ResponseTimes { get; set; } = new(); // in milliseconds
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan Duration => EndTime?.Subtract(StartTime) ?? TimeSpan.Zero;
    
    public double AverageResponseTime => ResponseTimes.Count > 0 ? ResponseTimes.Average() : 0;
    public long MinResponseTime => ResponseTimes.Count > 0 ? ResponseTimes.Min() : 0;
    public long MaxResponseTime => ResponseTimes.Count > 0 ? ResponseTimes.Max() : 0;
    public double RequestsPerSecond => Duration.TotalSeconds > 0 ? TotalRequests / Duration.TotalSeconds : 0;
}