﻿using Microsoft.SemanticKernel;

namespace StellarChat.Server.Api.Features.Chat.CarryConversation;

internal sealed class CarryConversationHandler : ICommandHandler<Ask, string>
{
    private const string ChatPluginName = nameof(ChatPlugin);
    private const string ChatFunctionName = "Chat";

    private readonly Kernel _kernel;

    public CarryConversationHandler(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async ValueTask<string> Handle(Ask command, CancellationToken cancellationToken)
    {
        var contextVariables = GetContextArguments(command);

        KernelFunction? chatFunction = _kernel.Plugins.GetFunction(ChatPluginName, ChatFunctionName);
        var result = await _kernel.InvokeAsync(chatFunction!, contextVariables, cancellationToken);

        return result.ToString();
    }

    private KernelArguments GetContextArguments(Ask command)
    {
        var contextArguments = new KernelArguments();

        contextArguments.TryAdd("message", command.Message);
        contextArguments.TryAdd("messageType", command.MessageType);
        contextArguments.TryAdd("model", command.Model);
        contextArguments.TryAdd("chatId", command.ChatId);

        return contextArguments;
    }
}
