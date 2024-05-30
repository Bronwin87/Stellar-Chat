﻿namespace StellarChat.Shared.Contracts.Actions;

public record Webhook(
    string httpMethod,
    string url,
    string? payload,
    bool isRetryEnabled,
    int retryCount,
    TimeSpan retryInterval,
    bool isScheduled,
    string? cronExpression,
    Dictionary<string, string>? headers = null);
