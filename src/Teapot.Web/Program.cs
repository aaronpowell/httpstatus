using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Teapot.Web;
using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSingleton<AmazonStatusCodeMetadata>();
builder.Services.AddSingleton<CloudflareStatusCodeMetadata>();
builder.Services.AddSingleton<EsriStatusCodeMetadata>();
builder.Services.AddSingleton<LaravelStatusCodeMetadata>();
builder.Services.AddSingleton<MicrosoftStatusCodeMetadata>();
builder.Services.AddSingleton<NginxStatusCodeMetadata>();
builder.Services.AddSingleton<TwitterStatusCodeMetadata>();

builder.Services.AddSingleton<TeapotStatusCodeMetadataCollection>();
builder.Services.AddApplicationInsightsTelemetry();

//builder.Services.AddFairUseRateLimiter();

builder.Services.AddCors();

WebApplication app = builder.Build();

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

//app.UseFairUseRateLimiter();

app.MapStatusEndpoints(FairUseRateLimiterExtensions.PolicyName);

app.MapRazorPages();

app.Run();
