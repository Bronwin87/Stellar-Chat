﻿namespace StellarChat.Server.Api.DAL.Mongo.Seeders;

public class AppSettingsSeeder(ILogger<AppSettingsSeeder> logger, TimeProvider clock) : IAppSettingsSeeder
{
    private readonly ILogger<AppSettingsSeeder> _logger = logger;
    private readonly TimeProvider _clock = clock;

    public async Task SeedAsync(IMongoDatabase database)
    {
        var settingsCollection = database.GetCollection<AppSettingsDocument>("settings");
        var cursor = await settingsCollection.FindAsync(FilterDefinition<AppSettingsDocument>.Empty);
        var documents = await cursor.ToListAsync();

        if (documents.Count == 0)
        {
            await SeedNewSettingsAsync(settingsCollection);
            return;
        }

        var appSettingsDocument = documents.First();
        
        if (appSettingsDocument.Integrations is null || appSettingsDocument.Integrations.Count == 0)
        {
            await UpdateSettingsWithIntegrationsAsync(settingsCollection, appSettingsDocument);
        }
    }

    private async Task SeedNewSettingsAsync(IMongoCollection<AppSettingsDocument> settingsCollection)
    {
        var now = _clock.GetLocalNow();

        var appSettingsDocument = new AppSettingsDocument
        {
            Id = Guid.NewGuid(),
            Profile = new ProfileDocument
            {
                Name = "User",
                AvatarUrl = "https://raw.githubusercontent.com/ktutak1337/Stellar-Chat/main/docs/assets/logo-small.jpg",
                Description = string.Empty,
            },
            Integrations =
            [
                new() {
                    Name = "OpenAI",
                    ApiKey = string.Empty,
                    Endpoint = string.Empty,
                    IsEnabled = true,
                },
                new() {
                    Name = "Ollama",
                    ApiKey = string.Empty,
                    Endpoint = string.Empty,
                    IsEnabled = true,
                },
            ],
            CreatedAt = now,
            UpdatedAt = now
        };

        _logger.LogInformation("Started seeding 'settings' collection.");

        await settingsCollection.InsertOneAsync(appSettingsDocument);

        _logger.LogInformation($"Added a settings document to the database with 'ID': {appSettingsDocument.Id}, and 'Key': {appSettingsDocument.Key}.");
        _logger.LogInformation("Finished seeding 'settings' collection.");
    }

    private async Task UpdateSettingsWithIntegrationsAsync(IMongoCollection<AppSettingsDocument> settingsCollection, AppSettingsDocument appSettingsDocument)
    {
        appSettingsDocument.Integrations = new List<IntegrationDocument>
        {
            new() 
            {
                Name = "OpenAI",
                ApiKey = string.Empty,
                Endpoint = string.Empty,
                IsEnabled = true,
            },
            new()
            {
                Name = "Ollama",
                ApiKey = string.Empty,
                Endpoint = string.Empty,
                IsEnabled = true,
            },
        };
        appSettingsDocument.UpdatedAt = _clock.GetLocalNow();
        _logger.LogInformation("Started updating 'settings' collection with integrations.");
        await settingsCollection.ReplaceOneAsync(doc => doc.Id == appSettingsDocument.Id, appSettingsDocument);
        _logger.LogInformation($"Updated the settings document in the database with 'ID': {appSettingsDocument.Id}.");
        _logger.LogInformation("Finished updating 'settings' collection.");
    }
}
