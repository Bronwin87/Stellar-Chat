﻿using StellarChat.Client.Web.Shared;
using StellarChat.Shared.Contracts.Assistants;
using System.Net.Http.Json;

namespace StellarChat.Client.Web.Services.Assistants;

public class AssistantService : IAssistantService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AssistantService(IHttpClientFactory httpClientFactory) 
        => _httpClientFactory = httpClientFactory;


    public async ValueTask<AssistantResponse> GetAssistant(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("WebAPI");

        var result = await httpClient.GetFromJsonAsync<AssistantResponse>($"/assistants/{id}");

        return result!;
    }

    public async ValueTask<Paged<AssistantResponse>> BrowseAssistants(int page = 0, int pageSize = 10000)
    {
        var httpClient = _httpClientFactory.CreateClient("WebAPI");

        var result = await httpClient.GetFromJsonAsync<Paged<AssistantResponse>>($"/assistants?Page={page}&PageSize={pageSize}");

        return result!;
    }

    public async ValueTask CreateAssistant(AssistantResponse assistant)
    {
        var httpClient = _httpClientFactory.CreateClient("WebAPI");

        var payload = new CreateAssistantRequest(
            assistant.Id, 
            assistant.Name, 
            assistant.Metaprompt, 
            assistant.Description, 
            assistant.AvatarUrl, 
            assistant.DefaultModel, 
            assistant.DefaultVoice, 
            assistant.IsDefault);

        await httpClient.PostAsJsonAsync($"/assistants", payload);
    }

    public async ValueTask UpdateAssistant(AssistantResponse assistant)
    {
        var httpClient = _httpClientFactory.CreateClient("WebAPI");
        
        var assistantId = assistant.Id;

        var payload = new UpdateAssistantRequest(
            assistantId,
            assistant.Name,
            assistant.Metaprompt,
            assistant.Description,
            assistant.AvatarUrl,
            assistant.DefaultModel,
            assistant.DefaultVoice,
            assistant.IsDefault);

        await httpClient.PutAsJsonAsync($"/assistants/{assistantId}", payload);
    }

    public async ValueTask SetDefaultAssistant(Guid id, bool isDefault)
    {
        var httpClient = _httpClientFactory.CreateClient("WebAPI");

        var payload = new SetDefaultAssistantRequest(id, isDefault);

        await httpClient.PutAsJsonAsync($"/assistants/{id}/default", payload);
    }

    public async ValueTask DeleteAssistant(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("WebAPI");

        await httpClient.DeleteAsync($"/assistants/{id}");
    }
}
