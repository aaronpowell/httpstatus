using Teapot.LoadTestUI.Components;
using Teapot.LoadTestUI.Services;

var builder = WebApplication.CreateBuilder(args);

// only configure OTEL not the rest of the service defaults
// since we don't want retries or anything here
builder.ConfigureOpenTelemetry();
builder.AddDefaultHealthChecks();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add HTTP client factory for load testing
builder.Services.AddHttpClient();

// Add load test service
builder.Services.AddScoped<LoadTestService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
