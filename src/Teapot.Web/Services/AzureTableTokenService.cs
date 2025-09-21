using System;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Caching.Memory;

namespace Teapot.Web.Services;

/// <summary>
/// Token validation using Azure Table Storage.
/// Expects entities stored under PartitionKey = "Token" and RowKey = the token GUID string.
/// The table entity is expected to contain columns: Created (DateTimeOffset), ContactEmail (string), PermittedLimit (int).
/// </summary>
public class AzureTableTokenService : ITokenService
{
    private readonly TableServiceClient tableServiceClient;
    private readonly IMemoryCache cache;
    private readonly TimeSpan cacheDuration = TimeSpan.FromMinutes(5);
    private const string PartitionKey = "Token";

    public AzureTableTokenService(TableServiceClient tableClient, IMemoryCache memoryCache)
    {
        tableServiceClient = tableClient ?? throw new ArgumentNullException(nameof(tableClient));
        cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        tableServiceClient.CreateTableIfNotExists(PartitionKey);
    }

    /// <inheritdoc />
    public Task<(bool Success, TokenAuthenticationInfo? Info)> TryGetAuthenticationAsync(Guid token)
    {
        if (token == Guid.Empty) return Task.FromResult((false, (TokenAuthenticationInfo?)null));

        var rowKey = token.ToString();
        var cacheKey = $"token:{rowKey}";

        return cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            try
            {
                var tableClient = tableServiceClient.GetTableClient(PartitionKey);
                var response = await tableClient.GetEntityAsync<TableEntity>(PartitionKey, rowKey).ConfigureAwait(false);
                var entity = response.Value;

                // Read fields with fallbacks
                DateTimeOffset created = DateTimeOffset.UtcNow;
                if (entity.TryGetValue("Created", out var createdObj) && createdObj is DateTimeOffset dto)
                {
                    created = dto;
                }
                else if (entity.TryGetValue("Created", out var createdObj2) && createdObj2 is DateTime dt)
                {
                    created = new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Utc));
                }

                string? email = null;
                if (entity.TryGetValue("ContactEmail", out var emailObj) && emailObj is string s)
                {
                    email = s;
                }

                int permitted = 100;
                if (entity.TryGetValue("PermittedLimit", out var permittedObj))
                {
                    if (permittedObj is int i) permitted = i;
                    else if (permittedObj is long l) permitted = (int)l;
                    else if (permittedObj is string ps && int.TryParse(ps, out var pi)) permitted = pi;
                }

                var info = new TokenAuthenticationInfo(created, email, permitted);

                // Found: cache for cacheDuration
                entry.AbsoluteExpirationRelativeToNow = cacheDuration;
                return (true, info);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // Not found: cache for a short period
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return (false, (TokenAuthenticationInfo?)null);
            }
        });
    }
}
