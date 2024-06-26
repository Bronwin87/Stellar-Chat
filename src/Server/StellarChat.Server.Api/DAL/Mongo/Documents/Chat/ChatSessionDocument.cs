﻿namespace StellarChat.Server.Api.DAL.Mongo.Documents.Chat;

internal class ChatSessionDocument : IIdentifiable<Guid>
{
    public Guid Id { get; set; }
    public Guid AssistantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Metaprompt { get; set; } = string.Empty;
    public HashSet<string> ActivePlugins { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
