using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Teapot.Web.Models;

namespace Teapot.Web;

public static class AppInfoExtensions
{
    public static WebApplicationBuilder AddAppInfo(this WebApplicationBuilder app)
    {
        app.Services.AddSingleton((_) =>
        {
            var assembly = typeof(AppInfoExtensions).Assembly;
            var assemblyName = assembly.GetName().Name;
            var gitVersionInformationType = assembly.GetType("GitVersionInformation");

            if (gitVersionInformationType is null)
            {
                return new AppInfo("Unknown", "Unknown", "Unknown", "Unknown", "Unknown", "Unknown");
            }

            var fields = gitVersionInformationType.GetFields();

            var sha = fields.First(f => f.Name == "Sha").GetValue(null);
            var shortSha = fields.First(f => f.Name == "ShortSha").GetValue(null);
            var preReleaseTag = fields.First(f => f.Name == "PreReleaseTag").GetValue(null);
            var fullBuildMetaData = fields.First(f => f.Name == "FullBuildMetaData").GetValue(null);
            var buildMetadata = fields.First(f => f.Name == "FullBuildMetaData").GetValue(null);
            var commitDate = fields.First(f => f.Name == "CommitDate").GetValue(null);

            return new AppInfo(
                sha?.ToString() ?? "Unknown",
                shortSha?.ToString() ?? "Unknown",
                preReleaseTag?.ToString() ?? "",
                fullBuildMetaData?.ToString() ?? "Unknown",
                buildMetadata?.ToString() ?? "Unknown",
                commitDate?.ToString() ?? "Unknown"
            );
        });
        return app;
    }
}
