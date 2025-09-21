using Microsoft.AspNetCore.Components;
using Teapot.LoadTestUI.Models;
using Teapot.LoadTestUI.Services;

namespace Teapot.LoadTestUI.Components.Pages;

public partial class Home
{
    [Inject] public required LoadTestService LoadTestService { get; set; }

    private LoadTestRequest loadTestRequest = null!;

    private LoadTestResult? loadTestResult;
    private bool isRunning;
    private string errorMessage = string.Empty;
    private (int completed, int total)? progress;
    private CancellationTokenSource? cancellationTokenSource;

    protected override void OnInitialized()
    {
        loadTestRequest = new()
        {
            TargetUrl = "http://localhost:5110/200",
            ConcurrentUsers = 5,
            RequestsPerUser = 10,
            HttpMethod = "GET"
        };
    }

    private async Task StartLoadTest()
    {
        if (string.IsNullOrWhiteSpace(loadTestRequest.TargetUrl))
        {
            errorMessage = "Please enter a valid target URL.";
            return;
        }

        isRunning = true;
        errorMessage = string.Empty;
        loadTestResult = null;
        progress = null;
        cancellationTokenSource = new CancellationTokenSource();

        try
        {
            var progressReporter = new Progress<(int completed, int total)>(p =>
            {
                progress = p;
                InvokeAsync(StateHasChanged);
            });

            loadTestResult = await LoadTestService.ExecuteLoadTestAsync(
                loadTestRequest,
                progressReporter,
                cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            errorMessage = "Load test was cancelled.";
        }
        catch (Exception ex)
        {
            errorMessage = $"Error during load test: {ex.Message}";
        }
        finally
        {
            isRunning = false;
            progress = null;
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }
    }

    private void CancelLoadTest()
    {
        cancellationTokenSource?.Cancel();
    }

    public void Dispose()
    {
        cancellationTokenSource?.Dispose();
    }
}