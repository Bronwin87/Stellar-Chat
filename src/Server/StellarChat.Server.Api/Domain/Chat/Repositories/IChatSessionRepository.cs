﻿namespace StellarChat.Server.Api.Domain.Chat.Repositories;

internal interface IChatSessionRepository
{
    ValueTask<ChatSession?> GetAsync(Guid id);
    ValueTask<ChatSession?> GetByTitleAsync(string title);
    ValueTask<IEnumerable<ChatSession>> BrowseAsync();
    ValueTask AddAsync(ChatSession chatSession);
    ValueTask UpdateAsync(ChatSession chatSession);
    ValueTask DeleteAsync(Guid id);
    ValueTask<bool> ExistsAsync(Guid id);
}
