using System.Text.Json;
using HonkBot.Models.Tools;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HonkBot.Services;

/// <summary>
/// An Azure Cosmos DB service that can be used to interact with the database.
/// </summary>
public partial class CosmosDbService : ICosmosDbService
{
    /// <summary>
    /// Config data passed in from dependency injection.
    /// </summary>
    private readonly IConfiguration _config;

    /// <summary>
    /// An <see cref="ILogger" /> for logging.
    /// </summary>
    private readonly ILogger<CosmosDbService> _logger;

    /// <summary>
    /// The database ID for the <see cref="CosmosDbService" /> to use.
    /// </summary>
    private readonly string _databaseId;

    private readonly CosmosDbJsonSerializer _cosmosDbJsonSerializer = new(
        new()
    );

    /// <summary>
    /// Default constructor for <see cref="CosmosDbService" />.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="logger"></param>
    public CosmosDbService(IConfiguration config, ILogger<CosmosDbService> logger)
    {
        _config = config;
        _logger = logger;
        string? retrievedDatabaseIdAppSetting = _config.GetValue<string>("CosmosDbDatabaseId");

        if (retrievedDatabaseIdAppSetting is null)
        {
            throw new ArgumentNullException("The 'CosmosDbDatabaseId' app setting was null. Failed to initialize CosmosDbService.");
        }

        _databaseId = retrievedDatabaseIdAppSetting;

        _cosmosDbClient = new(
            connectionString: _config.GetValue<string>("CosmosDbConnectionString"),
            clientOptions: new()
            {
                Serializer = _cosmosDbJsonSerializer
            }
        );
    }

    private CosmosClient _cosmosDbClient;
}