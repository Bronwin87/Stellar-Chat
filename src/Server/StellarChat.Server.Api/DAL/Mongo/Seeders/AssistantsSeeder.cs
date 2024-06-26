﻿namespace StellarChat.Server.Api.DAL.Mongo.Seeders;

internal sealed class AssistantsSeeder(ILogger<AssistantsSeeder> logger, TimeProvider clock) : IAssistantsSeeder
{
    private readonly ILogger<AssistantsSeeder> _logger = logger;
    private readonly TimeProvider _clock = clock;

    public async Task SeedAsync(IMongoDatabase database)
    {
        var settingsCollection = database.GetCollection<AssistantDocument>("assistants");
        var cursor = await settingsCollection.FindAsync(FilterDefinition<AssistantDocument>.Empty);
        var documents = await cursor.ToListAsync();

        if (documents.Any())
        {
            return;
        }

        var now = _clock.GetLocalNow();

        string metaprompt = @"You are an AI assistant designed for ultra-concise, engaging conversations. You are chatting with the user via Stellar Chat app.
RULES:
- Format responses in Markdown or JSON, like `**bold**`
- Always wrap code with triple backticks and keywords with `single backticks`
- Never use emojis or JSON format response, like `{""key"": ""value""}`, unless explicitly asked to
- Never repeat info or mention limitations
- Ask the user for clarification to avoid making unwarranted assumptions
Current date: {DATE}";

        var document = new AssistantDocument
        {
            Id = Guid.NewGuid(),
            Name = "Sophia",
            Description = "An AI assistant for seamless chat and accurate answers",
            AvatarUrl = "https://raw.githubusercontent.com/ktutak1337/Stellar-Chat/main/docs/assets/images/_ed19c10c-bbad-4514-8df4-eef37400e218.jpg",
            IsDefault = true,
            Metaprompt= metaprompt,
            DefaultModel = "gpt-4o",
            DefaultVoice = "Nova",
            CreatedAt = now,
            UpdatedAt = now,
        };

        _logger.LogInformation("Started seeding 'assistants' collection.");

        await settingsCollection.InsertOneAsync(document);

        _logger.LogInformation($"Added a default assistant to the database with 'ID': {document.Id}, and 'Name': {document.Name}.");
        _logger.LogInformation("Finished seeding 'assistants' collection.");
    }
}
