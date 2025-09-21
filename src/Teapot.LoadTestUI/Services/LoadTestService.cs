using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using Teapot.LoadTestUI.Models;

namespace Teapot.LoadTestUI.Services;

public class LoadTestService(IHttpClientFactory httpClientFactory, ILogger<LoadTestService> logger)
{
    public async Task<LoadTestResult> ExecuteLoadTestAsync(
        LoadTestRequest request,
        IProgress<(int completed, int total)>? progress = null,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting load test: {ConcurrentUsers} users, {RequestsPerUser} requests per user, Target URL: {TargetUrl}",
            request.ConcurrentUsers, request.RequestsPerUser, request.TargetUrl);

        var result = new LoadTestResult
        {
            StartTime = DateTime.UtcNow,
            TotalRequests = request.ConcurrentUsers * request.RequestsPerUser
        };

        using var activity = Diagnostics.ActivitySource.StartActivity($"Load Test Run - {request.ConcurrentUsers} users");
        if (activity is not null)
        {
            activity.SetTag("loadtest.targetUrl", request.TargetUrl);
            activity.SetTag("loadtest.concurrentUsers", request.ConcurrentUsers);
            activity.SetTag("loadtest.requestsPerUser", request.RequestsPerUser);
            activity.SetTag("loadtest.totalRequests", result.TotalRequests);
        }

        var responseTimes = new ConcurrentBag<long>();
        var statusCodes = new ConcurrentBag<HttpStatusCode>();
        var successCount = 0;
        var failureCount = 0;
        var completedRequests = 0;

        // Create tasks for each user
        var userTasks = new List<Task>();

        for (int userId = 0; userId < request.ConcurrentUsers; userId++)
        {
            var currentUserId = userId;
            var userTask = Task.Run(async () =>
            {
                using var userActivity = Diagnostics.ActivitySource.StartActivity($"User: {currentUserId}", ActivityKind.Client, parentContext: activity!.Context);
                if (userActivity is not null)
                {
                    userActivity.SetTag("loadtest.userId", currentUserId);
                    userActivity.SetTag("loadtest.requestsPerUser", request.RequestsPerUser);
                    userActivity.SetTag("loadtest.targetUrl", request.TargetUrl);
                    userActivity.SetTag("loadtest.httpMethod", request.HttpMethod);
                }

                using var httpClient = httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                for (int requestId = 0; requestId < request.RequestsPerUser; requestId++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    try
                    {
                        var stopwatch = Stopwatch.StartNew();
                        var httpRequestMessage = CreateHttpRequestMessage(request);

                        var response = await httpClient.SendAsync(httpRequestMessage, cancellationToken);
                        stopwatch.Stop();

                        responseTimes.Add(stopwatch.ElapsedMilliseconds);
                        statusCodes.Add(response.StatusCode);

                        if (response.IsSuccessStatusCode)
                        {
                            Interlocked.Increment(ref successCount);
                        }
                        else
                        {
                            Interlocked.Increment(ref failureCount);
                        }
                    }
                    catch (Exception)
                    {
                        Interlocked.Increment(ref failureCount);
                        statusCodes.Add(HttpStatusCode.InternalServerError);
                        logger.LogWarning("Request failed for user {UserId}, request {RequestId}", currentUserId, requestId);
                    }

                    Interlocked.Increment(ref completedRequests);
                    progress?.Report((completedRequests, result.TotalRequests));
                    logger.LogInformation("Completed {CompletedRequests}/{TotalRequests} requests for {UserId}", completedRequests, result.TotalRequests, currentUserId);
                }
            }, cancellationToken);

            userTasks.Add(userTask);
        }

        await Task.WhenAll(userTasks);

        result.EndTime = DateTime.UtcNow;
        result.SuccessfulRequests = successCount;
        result.FailedRequests = failureCount;
        result.ResponseTimes = [.. responseTimes];
        result.StatusCodes = [.. statusCodes];

        // Add final metrics to the activity and set status
        if (activity is not null)
        {
            activity.SetTag("loadtest.successfulRequests", result.SuccessfulRequests);
            activity.SetTag("loadtest.failedRequests", result.FailedRequests);

            var durationMs = (result?.EndTime - result?.StartTime)?.TotalMilliseconds;
            activity.SetTag("loadtest.durationMs", durationMs);

            if (cancellationToken.IsCancellationRequested)
            {
                activity.SetTag("loadtest.cancelled", true);
            }

            if (result?.FailedRequests > 0)
            {
                activity.SetStatus(ActivityStatusCode.Error, $"FailedRequests={result.FailedRequests}");
            }
            else
            {
                activity.SetStatus(ActivityStatusCode.Ok);
            }
        }

        return result!;
    }

    private static HttpRequestMessage CreateHttpRequestMessage(LoadTestRequest request)
    {
        var httpMethod = request.HttpMethod.ToUpperInvariant() switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            "HEAD" => HttpMethod.Head,
            "PATCH" => HttpMethod.Patch,
            _ => HttpMethod.Get
        };

        var httpRequestMessage = new HttpRequestMessage(httpMethod, request.TargetUrl);

        if (!string.IsNullOrWhiteSpace(request.Token))
        {
            httpRequestMessage.Headers.Add("x-httpstatus-token", request.Token);
        }

        return httpRequestMessage;
    }
}