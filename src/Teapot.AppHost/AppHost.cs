using Azure.Provisioning.ApplicationInsights;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(azurite =>
    {
        azurite.WithDataVolume();
    });

var tokensTable = storage.AddTables("tokens");

var teapot = builder.AddProject<Projects.Teapot_Web>("teapot-web")
    .WithReference(tokensTable)
    .WaitFor(tokensTable)
    .WithHttpHealthCheck("/200")
    .WithEnvironment("UseAzureTableTokenService", "true")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Teapot_LoadTestUI>("loadtest-ui")
    .ExcludeFromManifest();

if (builder.ExecutionContext.IsPublishMode)
{
    var appInsights = builder.AddAzureApplicationInsights("app-insights").ConfigureInfrastructure(infra =>
    {
        var resources = infra.GetProvisionableResources();
        var appInsightsResource = resources.OfType<ApplicationInsightsComponent>().FirstOrDefault();

        if (appInsightsResource is not null)
        {
            appInsightsResource.IsDisableIPMasking = true;
        }
    });
    teapot.WithReference(appInsights);

    builder.AddAzureAppServiceEnvironment("appsvc-httpstatus");

    teapot.PublishAsAzureAppServiceWebsite((infra, site) =>
    {
    });
}

builder.Build().Run();
