# HTTP Load Test UI

A web-based load testing tool built with Blazor Server that allows you to test HTTP endpoints with configurable concurrent users and request patterns.

## Features

### 🎯 Flexible Target Configuration
- **Target URL**: Test any HTTP endpoint
- **HTTP Methods**: Support for GET, POST, PUT, DELETE, HEAD, and PATCH
- **Authentication**: Optional token support via `x-httpstatus-token` header

### ⚡ Load Testing Capabilities
- **Concurrent Users**: Simulate 1-1000 concurrent users
- **Requests per User**: Configure 1-1000 requests per user
- **Real-time Progress**: Visual progress bar during test execution
- **Cancellation**: Cancel running tests at any time

### 📊 Comprehensive Results
- **Performance Metrics**: Response times (avg, min, max), requests per second
- **Success/Failure Rates**: Count and percentage of successful vs failed requests
- **Status Code Distribution**: Breakdown of HTTP status codes returned
- **Test Duration**: Total execution time

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- ASP.NET Core runtime

### Running the Application

1. **Standalone Mode** (for testing):
   ```bash
   cd src/Teapot.LoadTestUI
   dotnet run
   ```
   Navigate to `https://localhost:5001` or `http://localhost:5000`

2. **Via Aspire AppHost** (recommended):
   ```bash
   cd src/Teapot.AppHost
   dotnet run
   ```
   The LoadTest UI will be available through the Aspire dashboard.

## Usage

### Basic Load Test
1. Navigate to the "Load Test" page
2. Enter your target URL (e.g., `https://localhost:7000/status/200`)
3. Configure the load parameters:
   - Concurrent Users: Number of simulated users
   - Requests per User: How many requests each user will make
   - HTTP Method: The HTTP verb to use
4. Optionally add a token for authentication
5. Click "Start Load Test"

### Sample Test Scenarios

#### Test httpstatus API
```
Target URL: https://localhost:7000/status/200
Concurrent Users: 10
Requests per User: 50
HTTP Method: GET
```

#### Test with Authentication
```
Target URL: https://localhost:7000/status/200
Concurrent Users: 5
Requests per User: 20
HTTP Method: GET
Token: 11111111-1111-1111-1111-111111111111
```

#### High-Load Scenario
```
Target URL: https://httpbin.org/status/200
Concurrent Users: 100
Requests per User: 10
HTTP Method: GET
```

## Architecture

### Components

#### Models
- **`LoadTestRequest`**: Configuration for load test parameters
- **`LoadTestResult`**: Results and metrics from completed tests

#### Services
- **`LoadTestService`**: Core service that orchestrates the load testing
  - Uses `HttpClientFactory` for efficient HTTP client management
  - Implements concurrent execution using `Task.Run`
  - Provides real-time progress reporting
  - Collects comprehensive metrics

#### UI Components
- **`LoadTest.razor`**: Main interactive page with form validation and results display
- **Real-time Updates**: Uses Blazor Server's SignalR connection for live progress updates
- **Responsive Design**: Bootstrap-based UI that works on desktop and mobile

### Key Features Implementation

#### Concurrent Execution
```csharp
// Creates separate tasks for each simulated user
for (int userId = 0; userId < request.ConcurrentUsers; userId++)
{
    var userTask = Task.Run(async () => {
        // Each user makes multiple requests
        for (int requestId = 0; requestId < request.RequestsPerUser; requestId++)
        {
            // Execute HTTP request and collect metrics
        }
    });
    userTasks.Add(userTask);
}
await Task.WhenAll(userTasks);
```

#### Real-time Progress
```csharp
var progress = new Progress<(int completed, int total)>(p =>
{
    // Update UI with current progress
    progress = p;
    InvokeAsync(StateHasChanged);
});
```

#### Metrics Collection
- **Response Times**: Measured using `Stopwatch` for each request
- **Status Codes**: Captured from HTTP responses
- **Success/Failure Tracking**: Thread-safe counters using `Interlocked`
- **Performance Calculations**: Requests per second, averages, min/max

## Configuration

### Default Settings
- Target URL: `https://localhost:7000/status/200`
- Concurrent Users: 5
- Requests per User: 10
- HTTP Method: GET
- Request Timeout: 30 seconds

### Validation Rules
- Target URL: Must be a valid URL
- Concurrent Users: 1-1000
- Requests per User: 1-1000
- HTTP Method: Required selection
- Token: Optional GUID format

## Error Handling

### Network Errors
- Connection timeouts
- DNS resolution failures
- Network unreachable

### HTTP Errors
- 4xx Client errors
- 5xx Server errors
- Custom error status codes

### Application Errors
- Invalid configuration
- Cancellation during execution
- Resource exhaustion

## Performance Considerations

### Resource Usage
- Each concurrent user runs in a separate task
- HTTP clients are managed by `HttpClientFactory` for connection pooling
- Memory usage scales with number of concurrent users and requests

### Limitations
- Maximum 1000 concurrent users (configurable)
- Maximum 1000 requests per user (configurable)
- 30-second timeout per request
- Results stored in memory during test execution

## Testing

The LoadTestService has been tested with:
- ✅ Basic HTTP GET requests
- ✅ Concurrent user simulation
- ✅ Progress reporting
- ✅ Error handling
- ✅ Metrics collection
- ✅ Token authentication

Example test output:
```
Testing https://httpbin.org/status/200 with 2 users, 3 requests each
Progress: 1/6 (16.7%)
Progress: 6/6 (100.0%)

=== Results ===
Total Requests: 6
Successful: 6
Failed: 0
Duration: 4.54s
Requests/sec: 1.3
Avg Response Time: 1238ms
Status Code Distribution:
  OK: 6 (100.0%)
```

## Future Enhancements

- [ ] Request body configuration for POST/PUT requests
- [ ] Custom headers configuration
- [ ] Export results to CSV/JSON
- [ ] Historical test comparison
- [ ] Graphs and charts for results visualization
- [ ] Distributed load testing across multiple machines
- [ ] WebSocket load testing support