using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

namespace Teapot.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<CloudflareStatusCodeResults>();
            services.AddSingleton<TeapotStatusCodeResults>();
            services.AddApplicationInsightsTelemetry();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Teapot");
            });
        }
    }
}
