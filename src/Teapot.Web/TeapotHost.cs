using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Teapot.Web;
using Teapot.Web.Configuration;
using Teapot.Web.Middleware;
using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;
using Teapot.Web.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorPages();

builder.Services.AddSingleton<AmazonStatusCodeMetadata>();
builder.Services.AddSingleton<CloudflareStatusCodeMetadata>();
builder.Services.AddSingleton<EsriStatusCodeMetadata>();
builder.Services.AddSingleton<LaravelStatusCodeMetadata>();
builder.Services.AddSingleton<MicrosoftStatusCodeMetadata>();
builder.Services.AddSingleton<NginxStatusCodeMetadata>();
builder.Services.AddSingleton<TwitterStatusCodeMetadata>();

builder.Services.AddSingleton<TeapotStatusCodeMetadataCollection>();

if (builder.Configuration.GetValue("UseAzureTableTokenService", false))
{
    // Use Azure Table Storage for token service
    builder.AddAzureTableServiceClient("tokens");
    // Add in-memory caching for token lookups
    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<ITokenService, AzureTableTokenService>();
}
else
{
    // Use in-memory token service for simplicity (not for production use)
    builder.Services.AddSingleton<ITokenService, InMemoryTokenService>();
}

// Configure timeout options
builder.Services.Configure<TimeoutOptions>(builder.Configuration.GetSection(TimeoutOptions.SectionName));
builder.Services.AddSingleton(provider => provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<TimeoutOptions>>().Value);

builder.Services.AddFairUseRateLimiter();

builder.Services.AddCors();

builder.AddAppInfo();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add user identity middleware before routing
app.UseMiddleware<UserIdentityMiddleware>();

// Reject requests that include timeout parameters when user is not authenticated
app.UseMiddleware<RequireAuthenticatedForTimeoutMiddleware>();

app.UseRouting();

app.UseCors(builder =>
{
    builder
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .WithExposedHeaders(
        [
            "Link", // 103
            "Content-Range", // 206
            "Location", // 301, 302, 303, 305, 307, 308
            "WWW-Authenticate", // 401
            "Proxy-Authenticate", // 407
            "Retry-After" // 429
        ]);
});

app.UseFairUseRateLimiter();

app.MapStatusEndpoints(FairUseRateLimiterExtensions.PolicyName);

app.MapRazorPages();

app.Run();
