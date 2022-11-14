using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CloudflareStatusCodeResults>();
builder.Services.AddSingleton<TeapotStatusCodeResults>();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllersWithViews();

var app = builder.Build();

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
        .WithExposedHeaders(new[]
        {
                    "Link", // 103
                    "Content-Range", // 206
                    "Location", // 301, 302, 303, 305, 307, 308
                    "WWW-Authenticate", // 401
                    "Proxy-Authenticate", // 407
                    "Retry-After" // 429
        });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Teapot}/{action=teapot}");

app.Run();
