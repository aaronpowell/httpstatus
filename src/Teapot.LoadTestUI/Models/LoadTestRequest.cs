using System.ComponentModel.DataAnnotations;

namespace Teapot.LoadTestUI.Models;

public class LoadTestRequest
{
    [Required(ErrorMessage = "Target URL is required")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    public string TargetUrl { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "Concurrent users must be between 1 and 1000")]
    public int ConcurrentUsers { get; set; } = 1;

    [Range(1, 1000, ErrorMessage = "Requests per user must be between 1 and 1000")]
    public int RequestsPerUser { get; set; } = 1;

    public string? Token { get; set; }

    [Required(ErrorMessage = "HTTP Method is required")]
    public string HttpMethod { get; set; } = "GET";
}