using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Teapot.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //"I'm a teapot",
                //"im-a-teapot",
                //new { controller = "Teapot", action = "Teapot" });

                //routes.MapRoute(
                //"redirected",
                //"redirected/index/{statusCode}",null,
                //new { statusCode = @"\d{3}" });

                //routes.MapRoute(
                //"default",
                //"{statusCode}",
                //new { controller = "Status", action = "Status" },
                //new { statusCode = @"\d{3}" });



                //routes.MapRoute(
                //"redirected",
                //"redirected/{statusCode}",
                //new { controller = "Status" },
                //new { statusCode = @"\d{3}" });

                //routes.MapRoute(
                //        name: "homepage",
                //        template: "status/index",
                //        new { controller = "status"}

                routes.MapRoute(
                        name: "default",
                        template: "{controller=status}/{action=Index}/{statusCode?}");
            });
        }
    }
}
